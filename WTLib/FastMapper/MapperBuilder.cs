using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using WTLib.Inspector;
using WTLib.Unsafe;

namespace WTLib.FastMapper
{
    /// <summary>
    /// IL language based mapper builder.
    /// </summary>
    public static class MapperBuilder
    {
        internal static IBuildMapper CreateEmitBuildMapper(Type sourceType, Type targetType)
        {
            var assemblyName = new AssemblyName("EmitMapperBuilder");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var dynamicModule = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = dynamicModule.DefineType($"{sourceType.Name}To{targetType.Name}EmitMapper`2",
                TypeAttributes.Public | TypeAttributes.Class);
            typeBuilder.AddInterfaceImplementation(typeof(IEmitBuildMapper<,>).MakeGenericType(sourceType, targetType));
            var methodBuilder = typeBuilder.DefineMethod("Map", MethodAttributes.Public | MethodAttributes.Final |
                                                             MethodAttributes.Virtual | MethodAttributes.NewSlot |
                                                             MethodAttributes.HideBySig, targetType, new[] { sourceType });
            var ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.DeclareLocal(targetType);
            ilGenerator.DeclareLocal(targetType);
            ilGenerator.Emit(OpCodes.Newobj, targetType.GetConstructor(Type.EmptyTypes));
            ilGenerator.Emit(OpCodes.Stloc_0);

            var tProperties = targetType.GetProperties();
            var sProperties = sourceType.GetProperties();

            foreach (var tProperty in tProperties)
            {
                foreach (var sProperty in sProperties)
                {
                    if (tProperty.PropertyType == sProperty.PropertyType &&
                        tProperty.Name == sProperty.Name &&
                        tProperty.CanWrite &&
                        sProperty.CanRead)
                    {
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        ilGenerator.Emit(OpCodes.Callvirt, sourceType.GetMethod($"get_{sProperty.Name}"));
                        ilGenerator.Emit(OpCodes.Callvirt, targetType.GetMethod($"set_{tProperty.Name}"));
                    }
                }
            }
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Stloc_1);
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Ret);
            var generatedType = typeBuilder.CreateType();
            return (IBuildMapper)Activator.CreateInstance(generatedType);
        }

        public static IEmitBuildMapper<TSource, TTarget> CreateEmitBuildMapper<TSource, TTarget>()
        {
            return (IEmitBuildMapper<TSource, TTarget>)CreateEmitBuildMapper(typeof(TSource), typeof(TTarget));
        }

        internal static IBuildMapper CreateUnsafeBuildMapper(Type sourceType, Type targetType)
        {
            var assemblyName = new AssemblyName("UnsafeMapperBuilder");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var dynamicModule = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = dynamicModule.DefineType($"{sourceType.Name}To{targetType.Name}UnsafeMapper`2",
                TypeAttributes.Public | TypeAttributes.Class);
            typeBuilder.AddInterfaceImplementation(typeof(IUnsafeBuildMapper<,>).MakeGenericType(sourceType, targetType));
            var methodBuilder = typeBuilder.DefineMethod("Map", MethodAttributes.Public | MethodAttributes.Final |
                                                             MethodAttributes.Virtual | MethodAttributes.NewSlot |
                                                             MethodAttributes.HideBySig, targetType, new[] { sourceType });
            var ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.DeclareLocal(targetType);
            ilGenerator.DeclareLocal(typeof(int));
            ilGenerator.DeclareLocal(typeof(int));
            ilGenerator.DeclareLocal(typeof(int));
            ilGenerator.DeclareLocal(targetType);
            ilGenerator.Emit(OpCodes.Newobj, targetType.GetConstructor(Type.EmptyTypes));
            ilGenerator.Emit(OpCodes.Stloc_0);

            foreach (var (tFieldInfo, tOffset) in TypeInspector.GetFieldOffsets(targetType))
            {
                foreach (var (sFieldInfo, sOffset) in TypeInspector.GetFieldOffsets(sourceType))
                {
                    if (tFieldInfo.Name == sFieldInfo.Name && tFieldInfo.FieldType == sFieldInfo.FieldType)
                    {
                        int byteCount = tFieldInfo.FieldType.IsValueType
                            ? Marshal.SizeOf(Activator.CreateInstance(tFieldInfo.FieldType))
                            : UnsafeHelper.PointerSize;
                        ilGenerator.Emit(OpCodes.Ldc_I4, sOffset);
                        ilGenerator.Emit(OpCodes.Stloc_1);
                        ilGenerator.Emit(OpCodes.Ldc_I4, tOffset);
                        ilGenerator.Emit(OpCodes.Stloc_2);
                        ilGenerator.Emit(OpCodes.Ldc_I4, byteCount);
                        ilGenerator.Emit(OpCodes.Stloc_3);
                        ilGenerator.Emit(OpCodes.Ldarg_1);
                        ilGenerator.Emit(OpCodes.Ldloc_0);
                        ilGenerator.Emit(OpCodes.Ldloc_1);
                        ilGenerator.Emit(OpCodes.Ldloc_2);
                        ilGenerator.Emit(OpCodes.Ldloc_3);
                        ilGenerator.Emit(OpCodes.Call, typeof(UnsafeHelper).GetMethod(nameof(UnsafeHelper.CopyRefTypeField)).
                            MakeGenericMethod(new[] { sourceType, targetType }));
                    }
                }
            }
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Stloc_S, 4);
            ilGenerator.Emit(OpCodes.Ldloc_S, 4);
            ilGenerator.Emit(OpCodes.Ret);
            var generatedType = typeBuilder.CreateType();
            return (IBuildMapper)Activator.CreateInstance(generatedType);
        }

        public static IUnsafeBuildMapper<TSource, TTarget> CreateUnsafeBuildMapper<TSource, TTarget>()
        {
            return (IUnsafeBuildMapper<TSource, TTarget>)CreateUnsafeBuildMapper(typeof(TSource), typeof(TTarget));
        }
    }
}
