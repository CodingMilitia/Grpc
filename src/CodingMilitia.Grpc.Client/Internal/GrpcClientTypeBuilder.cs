using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CodingMilitia.Grpc.Shared;

namespace CodingMilitia.Grpc.Client.Internal
{
    internal class GrpcClientTypeBuilder
    {
        public TypeInfo Create<TService>() where TService : class, IGrpcService
        {

            var assemblyName = Guid.NewGuid().ToString();
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName);

            var serviceType = typeof(TService);
            var typeBuilder = moduleBuilder.DefineType(serviceType.Name + "Client", TypeAttributes.Public, typeof(GrpcClientBase));

            typeBuilder.AddInterfaceImplementation(serviceType);
            AddConstructor(typeBuilder, serviceType);
            AddMethods(typeBuilder, serviceType);

            return typeBuilder.CreateTypeInfo();
        }

        private void AddConstructor(TypeBuilder typeBuilder, Type serviceType)
        {
            var ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new[] { typeof(GrpcClientBaseOptions) }
            );

            var il = ctorBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0); //load this
            il.Emit(OpCodes.Ldarg_1); //load options
            var clientBaseType = typeof(GrpcClientBase);
            var ctorToCall = clientBaseType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(GrpcClientBaseOptions) }, null);
            il.Emit(OpCodes.Call, ctorToCall);//call base class method
            il.Emit(OpCodes.Ret);
        }

        private void AddMethods(TypeBuilder typeBuilder, Type serviceType)
        {
            foreach (var method in serviceType.GetMethods())
            {
                AddMethod(typeBuilder, method);
            }
        }

        private void AddMethod(TypeBuilder typeBuilder, MethodInfo method)
        {
            //ok, now I'm way out of my league...
            var args = method.GetParameters();
            var methodBuilder = typeBuilder.DefineMethod(
                method.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                method.ReturnType,
                (from arg in args select arg.ParameterType).ToArray()
            );
            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0); //load this
            il.Emit(OpCodes.Ldarg_1); //load request
            il.Emit(OpCodes.Ldarg_2); //load cancellation token
            var clientBaseType = typeof(GrpcClientBase);
            var methodToCall = clientBaseType.GetMethod("CallUnaryMethodAsync", BindingFlags.Instance | BindingFlags.NonPublic);

            il.Emit(
                OpCodes.Call,
                methodToCall.MakeGenericMethod(new[]{ //TODO: must check arguments and stuff
                    method.GetParameters()[0].ParameterType,
                    method.ReturnType.GetGenericArguments()[0]
                })
            ); //call base class method

            il.Emit(OpCodes.Ret); //return (the return value is already on the stack )

            typeBuilder.DefineMethodOverride(methodBuilder, method);
        }
    }
}