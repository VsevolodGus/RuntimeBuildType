﻿using System.Collections.ObjectModel;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;

namespace BuildObjects.Builds;

public class TypeFactory
{
    public Type CreateType(ReadOnlyCollection<DbColumn> columns)
    {
        int indexColumn = 0;

        var typeBuilder = FactoryTypeBuilder("DynamicType");
        while (indexColumn < columns.Count)
        {
            CreateProperty(typeBuilder, columns[indexColumn].ColumnName, columns[indexColumn].DataType);
            indexColumn++;
        }
        return typeBuilder.CreateType();
    }

    private TypeBuilder FactoryTypeBuilder(string name)
    {
        // 1. create assembly name
        var assemblyName = new AssemblyName($"SomeAssemblyName");

        // 2. create the assembly builder
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

        // 3. that is needed to create a module builder
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

        // 4. and finally our TypeBuilder (a public class)
        TypeBuilder tb = moduleBuilder.DefineType(name,
        TypeAttributes.Public |
        TypeAttributes.Class);

        return tb;
        //type.AddP
    }

    private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
    {
        if (propertyType == null) // when the propertytype is null, we assume, it's the Type itself (so we set it to the TypeBuilder)
            propertyType = tb;

        // same thing here for a IList with no generic type parameter -&amp;amp;amp;gt;
        // we assume it's the type (we could probably do that for every generic type without type parameter)
        //
        // if (propertyType == typeof(IList& amp; amp; amp; lt; &amp; amp; amp; gt;)) 
        //    propertyType = propertyType.MakeGenericType(tb);


        FieldBuilder fieldBuilder = tb.DefineField(propertyName, propertyType, FieldAttributes.Public); //creates the backing field
        PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

        //get method
        MethodBuilder getPropMthBldr = tb.DefineMethod(
           "get_" + propertyName
           , MethodAttributes.Public
            | MethodAttributes.SpecialName
            | MethodAttributes.HideBySig
           , /*returnType*/ propertyType
           , /*parameter types*/ Type.EmptyTypes); // see IL for the right MethodAttributes

        ILGenerator getIl = getPropMthBldr.GetILGenerator();
        // create the code in the get method
        getIl.Emit(OpCodes.Ldarg_0); //this
        getIl.Emit(OpCodes.Ldfld, fieldBuilder); //backingfield
        getIl.Emit(OpCodes.Ret);

        //set method
        MethodBuilder setPropMthdBldr =
        tb.DefineMethod(
           "set_" + propertyName,
           MethodAttributes.Public
            | MethodAttributes.SpecialName
            | MethodAttributes.HideBySig
           , /*returnType*/ null
           , /*parameter types*/ new[] { propertyType }); // see IL for the right MethodAttributes

        ILGenerator setIl = setPropMthdBldr.GetILGenerator();
        // create the code in the set method
        setIl.Emit(OpCodes.Ldarg_0); //this
        setIl.Emit(OpCodes.Ldarg_1); // 'value'
        setIl.Emit(OpCodes.Stfld, fieldBuilder); // backingfield

        setIl.Emit(OpCodes.Nop);
        setIl.Emit(OpCodes.Ret);

        //add methods to the propertyBuilder
        propertyBuilder.SetGetMethod(getPropMthBldr);
        propertyBuilder.SetSetMethod(setPropMthdBldr);
    }
}
