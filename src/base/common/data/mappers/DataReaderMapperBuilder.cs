﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using PropertyAttributes = System.Reflection.PropertyAttributes;

namespace Nohros.Data
{
  internal static class Dynamics_
  {
    const string kDynamicAssemblyName = "Nohros.Data.Dynamic";

    static readonly ModuleBuilder module_;
    static readonly IDictionary<string, MethodInfo> data_reader_methods_;

#if DEBUG
    static readonly AssemblyBuilder assembly_;
#endif

    #region .ctor
    static Dynamics_() {
      AppDomain app = AppDomain.CurrentDomain;
      AssemblyName asm_name = new AssemblyName();
      asm_name.Name = kDynamicAssemblyName;
#if DEBUG
      AssemblyBuilder asm_builder = app
        .DefineDynamicAssembly(asm_name, AssemblyBuilderAccess.RunAndSave);
      assembly_ = asm_builder;
#else
      AssemblyBuilder asm_builder = app
        .DefineDynamicAssembly(asm_name, AssemblyBuilderAccess.Run);
      module_ = asm_builder
        .DefineDynamicModule(asm_builder.GetName().Name, "nohros.data.dynamic.dll");
#endif
      module_ = asm_builder
        .DefineDynamicModule(asm_builder.GetName().Name, false);
      data_reader_methods_ = new Dictionary<string, MethodInfo>();
    }
    #endregion

#if DEBUG
    public static AssemblyBuilder AssemblyBuilder {
      get { return assembly_; }
    }
#endif

    /// <summary>
    /// Checks if a dynamic type exists for the type <paramref name="type"/>.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool DynamicTypeExists(Type type) {
      return module_.GetType(GetDynamicTypeName(type)) != null;
    }

    /// <summary>
    /// Gets the name of the dynamic type for the <typeparamref name="type"/>.
    /// </summary>
    /// <param name="type">
    /// The type which dynamic type name should be retrieved.
    /// </param>
    /// <returns></returns>
    public static string GetDynamicTypeName(Type type) {
      return type.Name + "_";
    }

    public static string GetDataReaderMethodName(Type type) {
      if (type == typeof (int)) {
        return "GetInt32";
      }
      if (type == typeof (string)) {
        return "GetString";
      }
      if (type == typeof (DateTime)) {
        return "GetDateTime";
      }
      if (type == typeof (bool)) {
        return "GetBoolean";
      }
      if (type == typeof (decimal)) {
        return "GetDecimal";
      }
      if (type == typeof (long)) {
        return "GetInt64";
      }
      if (type == typeof (Guid)) {
        return "GetGuid";
      }
      if (type == typeof (float)) {
        return "GetFloat";
      }
      if (type == typeof (double)) {
        return "GetDouble";
      }
      if (type == typeof (char)) {
        return "GetChar";
      }
      if (type == typeof (byte)) {
        return "GetByte";
      }
      if (type == typeof (short)) {
        return "GetInt16";
      }
      throw new ArgumentException(
        string
          .Format(Resources.Resources.Arg_WrongType, "type", "ValueType"));
    }

    public static MethodInfo GetDataReaderMethod(string method) {
      MethodInfo method_info;
      if (!data_reader_methods_.TryGetValue(method, out method_info)) {
        // Most of the IDataReader method is inherited from the IDataRecord
        // interface. We have more probability to found the requested
        // method on the IDataRecord interface than in the IDataReader
        // interface.
        method_info =
          typeof (IDataRecord).GetMethod(method) ??
            typeof (IDataReader).GetMethod(method);
      }
      return method_info;
    }

    public static ModuleBuilder ModuleBuilder {
      get { return module_; }
    }

    public static IDictionary<string, MethodInfo> DataReaderMethods {
      get { return data_reader_methods_; }
    }
  }

  public abstract partial class DataReaderMapper<T>
  {
    public class Builder
    {
      class MappingResult
      {
        public FieldBuilder OrdinalsField { get; set; }
        public KeyValuePair<int, PropertyInfo>[] OrdinalsMapping { get; set; }
        public FieldBuilder ReaderField { get; set; }
        public KeyValuePair<string, PropertyInfo>[] ValueMappings { get; set; }
        public PropertyInfo[] ReferenceMappings { get; set; }
        public KeyValuePair<ITypeMap, PropertyInfo>[] ConstantMappings { get; set; }
        public KeyValuePair<FieldBuilder, PropertyInfo>[] ReferenceFields { get; set; }
      }

      protected delegate void PreCreateTypeEventHandler(TypeBuilder builder);

      readonly IDictionary<string, ITypeMap> mappings_;

      readonly Type type_t_;

      #region .ctor
      /// <summary>
      /// Initializes a new instance of the <see cref="Builder"/> class.
      /// </summary>
      public Builder() : this(typeof (T)) {
      }

      Builder(Type type_t) {
        if (type_t.IsInterface == false) {
          throw new InvalidOperationException(
            string.Format(Resources.Resources.Arg_Type_ShouldBeInterface,
              type_t.Name));
        }
        mappings_ = new Dictionary<string, ITypeMap>(
          StringComparer.OrdinalIgnoreCase);
        type_t_ = type_t;
      }

      /// <summary>
      /// Builds a dynamic <see cref="DataReaderMapper{T}"/> for the type
      /// <typeparamref source="T"/> using the specified
      /// properties.
      /// </summary>
      /// <param name="mapping">
      /// An <see cref="KeyValuePair{TKey,TValue}"/> containing the mapping
      /// between source columns and destination properties.
      /// </param>
      /// <remarks>
      /// The <see cref="KeyValuePair{TKey,TValue}.Value"/> is used as the source
      /// column name and the <see cref="KeyValuePair{TKey,TValue}.Key"/> is
      /// used as the destination property name.
      /// </remarks>
      public Builder(IEnumerable<KeyValuePair<string, string>> mapping)
        : this(typeof (T)) {
        // Perform the mapping only if the dynamic type does not already exists
        if (!Dynamics_.DynamicTypeExists(type_t_)) {
          foreach (KeyValuePair<string, string> map in mapping) {
            Map(map.Key, map.Value);
          }
        }
      }

      /// <summary>
      /// Builds a dynamic <see cref="DataReaderMapper{T}"/> for the type
      /// <typeparamref source="T"/> using the specified
      /// properties.
      /// </summary>
      /// <param name="mapping">
      /// An <see cref="KeyValuePair{TKey,TValue}"/> containing the mapping
      /// between source columns and destination properties.
      /// </param>
      /// <remarks>
      /// The <see cref="KeyValuePair{TKey,TValue}.Value"/> is used as the source
      /// column name and the <see cref="KeyValuePair{TKey,TValue}.Key"/> is
      /// used as the destination property name.
      /// </remarks>
      public Builder(IEnumerable<KeyValuePair<string, ITypeMap>> mapping)
        : this(typeof (T)) {
        // Perform the mapping only if the dynamic type does not already exists
        if (!Dynamics_.DynamicTypeExists(type_t_)) {
          foreach (KeyValuePair<string, ITypeMap> map in mapping) {
            Map(map.Key, map.Value);
          }
        }
      }

      /// <summary>
      /// Builds a dynamic <see cref="DataReaderMapper{T}"/> for the type
      /// <typeparamref source="T"/> using the specified
      /// properties.
      /// </summary>
      /// <param name="mapping">
      /// An <see cref="KeyValuePair{TKey,TValue}"/> containing the mapping
      /// between source columns and destination properties
      /// </param>
      /// <remarks>
      /// The <see cref="KeyValuePair{TKey,TValue}.Value"/> is used as the source
      /// column name and the <see cref="KeyValuePair{TKey,TValue}.Key"/> is
      /// used as the destination property name.
      /// </remarks>
      public Builder(CallableDelegate<KeyValuePair<string, string>[]> mapping)
        : this(typeof (T)) {
        // Perform the mapping only if the dynamic type does not already exists
        if (!Dynamics_.DynamicTypeExists(type_t_)) {
          foreach (KeyValuePair<string, string> map in mapping()) {
            Map(map.Key, map.Value);
          }
        }
      }

      /// <summary>
      /// Builds a dynamic <see cref="DataReaderMapper{T}"/> for the type
      /// <typeparamref source="T"/> using the specified
      /// properties.
      /// </summary>
      /// <param name="mapping">
      /// An <see cref="KeyValuePair{TKey,TValue}"/> containing the mapping
      /// between source columns and destination properties
      /// </param>
      /// <remarks>
      /// The <see cref="KeyValuePair{TKey,TValue}.Value"/> is used as the source
      /// column name and the <see cref="KeyValuePair{TKey,TValue}.Key"/> is
      /// used as the destination property name.
      /// </remarks>
      public Builder(CallableDelegate<KeyValuePair<string, ITypeMap>[]> mapping)
        : this(typeof (T)) {
        // Perform the mapping only if the dynamic type does not already exists
        if (!Dynamics_.DynamicTypeExists(type_t_)) {
          foreach (KeyValuePair<string, ITypeMap> map in mapping()) {
            Map(map.Key, map.Value);
          }
        }
      }
      #endregion

      /// <summary>
      /// Maps the source column <paramref source="source"/> to the interface
      /// property <paramref source="destination"/>.
      /// </summary>
      /// <param name="source">
      /// The source of the source column.
      /// </param>
      /// <param name="destination">
      /// The source of the property that will be mapped to the column
      /// <paramref name="source"/>.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that builds an object of type
      /// <typeparamref source="T"/> and mapping the column <paramref source="source"/>
      /// to the property named <paramref source="destination"/>.
      /// </returns>
      public Builder Map(string destination, string source) {
        return Map(destination, new StringTypeMap(source));
      }

      /// <summary>
      /// Maps the constant value <see cref="value"/> to the interface
      /// property <paramref source="destination"/>.
      /// </summary>
      /// <param name="value">
      /// The value that should be returned when by the interface property
      /// <paramref name="destination"/>.
      /// </param>
      /// <param name="destination">
      /// The source of the property that will be mapped to the value
      /// <paramref name="value"/>.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that builds an object of type
      /// <typeparamref source="T"/> and mapping the constant value
      /// <paramref source="value"/> to the property named
      /// <paramref source="destination"/>.
      /// </returns>
      public Builder Map(string destination, int value) {
        return Map(destination, new IntMapType(value));
      }

      /// <summary>
      /// Maps the constant value <see cref="value"/> to the interface
      /// property <paramref source="destination"/>.
      /// </summary>
      /// <param name="value">
      /// The value that should be returned when by the interface property
      /// <paramref name="destination"/>.
      /// </param>
      /// <param name="destination">
      /// The source of the property that will be mapped to the value
      /// <paramref name="value"/>.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that builds an object of type
      /// <typeparamref source="T"/> and mapping the constant value
      /// <paramref source="value"/> to the property named
      /// <paramref source="destination"/>.
      /// </returns>
      public Builder Map(string destination, short value) {
        return Map(destination, new ShortMapType(value));
      }

      /// <summary>
      /// Maps the constant value <see cref="value"/> to the interface
      /// property <paramref source="destination"/>.
      /// </summary>
      /// <param name="value">
      /// The value that should be returned when by the interface property
      /// <paramref name="destination"/>.
      /// </param>
      /// <param name="destination">
      /// The source of the property that will be mapped to the value
      /// <paramref name="value"/>.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that builds an object of type
      /// <typeparamref source="T"/> and mapping the constant value
      /// <paramref source="value"/> to the property named
      /// <paramref source="destination"/>.
      /// </returns>
      public Builder Map(string destination, long value) {
        return Map(destination, new LongMapType(value));
      }

      /// <summary>
      /// Maps the constant value <see cref="value"/> to the interface
      /// property <paramref source="destination"/>.
      /// </summary>
      /// <param name="value">
      /// The value that should be returned when by the interface property
      /// <paramref name="destination"/>.
      /// </param>
      /// <param name="destination">
      /// The source of the property that will be mapped to the value
      /// <paramref name="value"/>.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that builds an object of type
      /// <typeparamref source="T"/> and mapping the constant value
      /// <paramref source="value"/> to the property named
      /// <paramref source="destination"/>.
      /// </returns>
      public Builder Map(string destination, float value) {
        return Map(destination, new FloatMapType(value));
      }

      Builder Map(string destination, ITypeMap value) {
        mappings_[destination] = value;
        return this;
      }

      /// <summary>
      /// Builds a dynamic <see cref="DataReaderMapper{T}"/> for the type
      /// <typeparamref source="T"/>.
      /// </summary>
      /// <returns>
      /// An object that implements the <see cref="IMapper{T}"/> and the
      /// <typeparamref source="T"/> interface and derives from the
      /// <see cref="DataReaderMapper{T}"/> class.
      /// </returns>
      public DataReaderMapper<T> Build(IDataReader reader) {
        return
          (DataReaderMapper<T>)
            Activator.CreateInstance(GetDynamicType(), reader);
      }

      /// <summary>
      /// Gets the dynamic type for <typeparamref name="T"/>.
      /// </summary>
      /// <param name="type">
      /// When this method returns contains the type that was dynamically
      /// created for the type <typeparamref name="T"/>, or <c>null</c> is
      /// a dynamic type for <typeparamref name="T"/> does not exists.
      /// </param>
      /// <returns></returns>
      public static bool TryGetDynamicType(out Type type) {
        string dynamic_type_name = Dynamics_.GetDynamicTypeName(typeof (T));
        type = Dynamics_.ModuleBuilder.GetType(dynamic_type_name);
        return type != null;
      }

      /// <summary>
      /// Gets the dynamic type for <typeparamref source="T"/>.
      /// </summary>
      /// <returns>
      /// The dynamic type for <typeparamref source="T"/>.
      /// </returns>
      /// <remarks>
      /// If the dynamic type does not already exists, it will be created.
      /// </remarks>
      Type GetDynamicType() {
        string dynamic_type_name = Dynamics_.GetDynamicTypeName(type_t_);
        return Dynamics_.ModuleBuilder
          .GetType(dynamic_type_name) ?? MakeDynamicType(dynamic_type_name);
      }

      /// <summary>
      /// Ignores a destination property, by not including it in the map.
      /// </summary>
      /// <param name="destination">
      /// The name of the destination property to be ignored.
      /// </param>
      /// <remarks>
      /// Ignored properties thrown an NotImplemented exception when an attempt
      /// to access is performed.
      /// </remarks>
      public Builder Ignore(string destination) {
        mappings_.Add(destination, null);
        return this;
      }

      /// <summary>
      /// Generates the dynamic type for <typeparamref source="T"/>.
      /// </summary>
      /// <returns>
      /// The dynamic type for <typeparamref source="T"/>.
      /// </returns>
      /// <remarks>
      /// The type is dynamically created and added to the current
      /// <see cref="AppDomain"/>.
      /// </remarks>
      Type MakeDynamicType(string dynamic_type_name) {
        TypeBuilder builder = Dynamics_.ModuleBuilder.DefineType(
          dynamic_type_name,
          TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AutoLayout,
          typeof (DataReaderMapper<T>),
          new Type[] {type_t_, typeof (IMapper<T>)});

        PropertyInfo[] properties = GetProperties();

        MappingResult result = new MappingResult();

        // Get the mappings for the properties that return value types.
        GetMappings(properties, result);

        // Get the mappings for the properties that return value types.
        result.ReferenceFields = EmitReferenceFields(builder,
          result.ReferenceMappings);

        EmitConstructor(builder, result);
        EmitMapMethod(builder);
        EmitReferenceProperties(builder, result.ReferenceFields);
        EmitValueProperties(builder, result.OrdinalsField, result.ReaderField,
          result.OrdinalsMapping);
        EmitConstantProperties(builder, result.ConstantMappings);

        OnPreCreateType(builder);

        return builder.CreateType();
      }

      void OnPreCreateType(TypeBuilder builder) {
        Listeners.SafeInvoke(PreCreateType,
          delegate(PreCreateTypeEventHandler handler) { handler(builder); });
      }

      /// <summary>
      /// Raised before the dynamic type is created.
      /// </summary>
      protected event PreCreateTypeEventHandler PreCreateType;

      /// <summary>
      /// Emit the constuctor code.
      /// </summary>
      void EmitConstructor(TypeBuilder type, MappingResult result) {
        ConstructorBuilder constructor =
          type.DefineConstructor(MethodAttributes.Public |
            MethodAttributes.HideBySig |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName,
            CallingConventions.Standard, new Type[] {typeof (IDataReader)});

        // Get the reference mappings and create its corresponding fields

        // Calls the constructor of the base class DataReaderMapper
        ConstructorInfo data_reader_mapper_ctor = typeof (DataReaderMapper<T>)
          .GetConstructor(
            BindingFlags.Public |
              BindingFlags.NonPublic |
              BindingFlags.Instance,
            null, new Type[] {typeof (IDataReader)}, null);
        ILGenerator il = constructor.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Call, data_reader_mapper_ctor);

        // Create the array that will store the column ordinals.
        result.OrdinalsField = type
          .DefineField("ordinals_", typeof (int[]), FieldAttributes.Private);

        result.ReaderField = type
          .DefineField("local_reader_", typeof (IDataReader),
            FieldAttributes.Private);

        // store the data reader
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Stfld, result.ReaderField);

        // get the columns ordinals.
        result.OrdinalsMapping =
          EmitOrdinals(il, result.OrdinalsField, result.ValueMappings);

        // create the reference objects.
        EmitReferencesInitialization(il, result.ReferenceFields);

        // return from the constructor
        il.Emit(OpCodes.Ret);
      }

      /// <summary>
      /// Emit the code that initialize the references mappings.
      /// </summary>
      /// <param name="il">
      /// The <see cref="ILGenerator"/> for the constructor method.
      /// </param>
      /// <param name="reference_fields">
      /// An array containing a map between a reference properties and its
      /// associated private field.
      /// </param>
      void EmitReferencesInitialization(ILGenerator il,
        KeyValuePair<FieldBuilder, PropertyInfo>[] reference_fields) {
        for (int i = 0, j = reference_fields.Length; i < j; i++) {
          FieldBuilder field = reference_fields[i].Key;
          PropertyInfo property = reference_fields[i].Value;

          // Create the reference type.
          Type generic_type_builder = typeof (DataReaderMapper<>.Builder);
          Type type_builder = generic_type_builder.MakeGenericType(new Type[] {
            property.PropertyType
          });
          object type_instance = Activator.CreateInstance(type_builder);
          Type reference_type = (Type) type_builder
            .InvokeMember("GetDynamicType", BindingFlags.InvokeMethod |
              BindingFlags.NonPublic | BindingFlags.Instance, null,
              type_instance, new object[0]);
          ConstructorInfo reference_type_ctor =
            reference_type.GetConstructor(new Type[] {
              typeof (IDataReader)
            });

          il.Emit(OpCodes.Ldarg_0); // load "this" pointer
          il.Emit(OpCodes.Ldarg_1); // load "reader" arg
          il.Emit(OpCodes.Newobj, reference_type_ctor); // new ref_type()
          il.Emit(OpCodes.Stfld, field); // store new type
        }
      }

      void EmitMapMethod(TypeBuilder type) {
        MethodBuilder builder = type
          .DefineMethod("Map",
            MethodAttributes.Public | MethodAttributes.HideBySig |
              MethodAttributes.Virtual, typeof (T), Type.EmptyTypes);

        ILGenerator il = builder.GetILGenerator();

        // load the this pointer into stack and return.
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ret);
      }

      KeyValuePair<FieldBuilder, PropertyInfo>[] EmitReferenceFields(
        TypeBuilder type, PropertyInfo[] reference_mappings) {
        int pos = 0;
        List<KeyValuePair<FieldBuilder, PropertyInfo>> forwarding_properties =
          new List<KeyValuePair<FieldBuilder, PropertyInfo>>(
            reference_mappings.Length);
        for (int i = 0, j = reference_mappings.Length; i < j; i++) {
          PropertyInfo property = reference_mappings[i];
          FieldBuilder field = type
            .DefineField(GetMemberName(property.Name), property.PropertyType,
              FieldAttributes.Private);
          forwarding_properties.Add(
            new KeyValuePair<FieldBuilder, PropertyInfo>(field, property));
        }
        return forwarding_properties.ToArray();
      }

      void EmitReferenceProperties(TypeBuilder type,
        KeyValuePair<FieldBuilder, PropertyInfo>[] reference_fields) {
        for (int i = 0, j = reference_fields.Length; i < j; i++) {
          FieldBuilder field = reference_fields[i].Key;
          PropertyInfo property = reference_fields[i].Value;
          PropertyBuilder builder = type
            .DefineProperty(property.Name, PropertyAttributes.HasDefault,
              property.PropertyType, null);

          MethodBuilder get_method = type.DefineMethod("get_" + property.Name,
            MethodAttributes.Public |
              MethodAttributes.SpecialName |
              MethodAttributes.HideBySig |
              MethodAttributes.Virtual |
              MethodAttributes.Final |
              MethodAttributes.NewSlot,
            property.PropertyType, Type.EmptyTypes);

          ILGenerator il = get_method.GetILGenerator();
          il.Emit(OpCodes.Ldarg_0);
          il.Emit(OpCodes.Ldfld, field);
          il.Emit(OpCodes.Ret);

          builder.SetGetMethod(get_method);
        }
      }

      void EmitValueProperties(TypeBuilder type, FieldBuilder ordinals,
        FieldBuilder reader, KeyValuePair<int, PropertyInfo>[] fields) {
        for (int i = 0, j = fields.Length; i < j; i++) {
          int ordinal = fields[i].Key;
          PropertyInfo property = fields[i].Value;
          PropertyBuilder builder = type
            .DefineProperty(property.Name, PropertyAttributes.HasDefault,
              property.PropertyType, null);

          MethodBuilder get_method = type.DefineMethod("get_" + property.Name,
            MethodAttributes.Public |
              MethodAttributes.SpecialName |
              MethodAttributes.HideBySig |
              MethodAttributes.Virtual |
              MethodAttributes.Final |
              MethodAttributes.NewSlot,
            property.PropertyType, Type.EmptyTypes);

          MethodInfo get_x_method =
            Dynamics_.GetDataReaderMethod(
              Dynamics_.GetDataReaderMethodName(property.PropertyType));

          ILGenerator il = get_method.GetILGenerator();
          il.Emit(OpCodes.Ldarg_0);
          il.Emit(OpCodes.Ldfld, reader);
          il.Emit(OpCodes.Ldarg_0);
          il.Emit(OpCodes.Ldfld, ordinals);
          il.Emit(OpCodes.Ldc_I4, ordinal);
          il.Emit(OpCodes.Ldelem_I4);
          il.Emit(OpCodes.Callvirt, get_x_method);
          il.Emit(OpCodes.Ret);

          builder.SetGetMethod(get_method);
        }
      }

      void EmitConstantProperties(TypeBuilder type,
        IEnumerable<KeyValuePair<ITypeMap, PropertyInfo>> constant_fields) {
        ConstructorInfo not_implemented_exception_ctor =
          typeof (NotImplementedException)
            .GetConstructor(BindingFlags.Instance | BindingFlags.Public,
              null, Type.EmptyTypes, null);
        foreach (KeyValuePair<ITypeMap, PropertyInfo> pair in constant_fields) {
          ITypeMap map = pair.Key;
          PropertyInfo property = pair.Value;

          PropertyBuilder builder = type
            .DefineProperty(property.Name, PropertyAttributes.HasDefault,
              property.PropertyType, null);

          MethodBuilder get_method = type.DefineMethod("get_" + property.Name,
            MethodAttributes.Public |
              MethodAttributes.SpecialName |
              MethodAttributes.HideBySig |
              MethodAttributes.Virtual |
              MethodAttributes.Final |
              MethodAttributes.NewSlot,
            property.PropertyType, Type.EmptyTypes);

          ILGenerator il = get_method.GetILGenerator();
          if (map.MapType == TypeMapType.Ignore) {
            il.Emit(OpCodes.Newobj, not_implemented_exception_ctor);
            il.Emit(OpCodes.Throw);
          } else {
            EmitLoad(il, map);
          }

          il.Emit(OpCodes.Ret);
          builder.SetGetMethod(get_method);
        }
      }

      void EmitLoad(ILGenerator il, ITypeMap map) {
        switch (map.MapType) {
          case TypeMapType.Int:
          case TypeMapType.Short:
            EmitLoadInt(il, (int) map.Value);
            break;

          case TypeMapType.Long:
            il.Emit(OpCodes.Ldc_I8, (long) map.Value);
            break;

          case TypeMapType.Boolean:
            if ((bool) map.Value) {
              il.Emit(OpCodes.Ldc_I4_1);
            }
            il.Emit(OpCodes.Ldc_I4_0);
            break;

          case TypeMapType.Byte:
            il.Emit(OpCodes.Ldc_I4, (byte) map.Value);
            break;

          case TypeMapType.Char:
            il.Emit(OpCodes.Ldc_I4, (char) map.Value);
            break;

          case TypeMapType.Double:
            il.Emit(OpCodes.Ldc_R8, (double) map.Value);
            break;

          case TypeMapType.Float:
            il.Emit(OpCodes.Ldc_R4, (float) map.Value);
            break;

          case TypeMapType.ConstString:
            il.Emit(OpCodes.Ldstr, (string) map.Value);
            break;
        }
      }

      void EmitLoadInt(ILGenerator il, int value) {
        if (value < 8 && value > -1) {
          switch (value) {
            case 0:
              il.Emit(OpCodes.Ldc_I4_0);
              break;
            case 1:
              il.Emit(OpCodes.Ldc_I4_1);
              break;
            case 2:
              il.Emit(OpCodes.Ldc_I4_2);
              break;
            case 3:
              il.Emit(OpCodes.Ldc_I4_3);
              break;
            case 4:
              il.Emit(OpCodes.Ldc_I4_4);
              break;
            case 5:
              il.Emit(OpCodes.Ldc_I4_5);
              break;
            case 6:
              il.Emit(OpCodes.Ldc_I4_6);
              break;
            case 7:
              il.Emit(OpCodes.Ldc_I4_7);
              break;
            case 8:
              il.Emit(OpCodes.Ldc_I4_8);
              break;
          }
        }
        il.Emit(OpCodes.Ldc_I4, value);
      }

      KeyValuePair<int, PropertyInfo>[] EmitOrdinals(ILGenerator il,
        FieldBuilder ordinals, KeyValuePair<string, PropertyInfo>[] fields) {
        KeyValuePair<int, PropertyInfo>[] ordinals_mapping =
          new KeyValuePair<int, PropertyInfo>[fields.Length];

        if (fields.Length > 0) {
          MethodInfo get_ordinal_method =
            Dynamics_.GetDataReaderMethod("GetOrdinal");

          il.Emit(OpCodes.Ldarg_0);
          il.Emit(OpCodes.Ldc_I4, fields.Length);
          il.Emit(OpCodes.Newarr, typeof (int));
          il.Emit(OpCodes.Stfld, ordinals);

          for (int i = 0, j = fields.Length; i < j; i++) {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, ordinals);
            il.Emit(OpCodes.Ldc_I4, i);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldstr, fields[i].Key);
            il.Emit(OpCodes.Callvirt, get_ordinal_method);
            il.Emit(OpCodes.Stelem_I4);

            ordinals_mapping[i] =
              new KeyValuePair<int, PropertyInfo>(i, fields[i].Value);
          }
        }
        return ordinals_mapping;
      }

      string GetMemberName(string name) {
        return name.ToLower() + "_";
      }

      bool IsReferenceType(PropertyInfo property) {
        Type type = property.PropertyType;
        if (type.IsValueType || type.Name == "String") {
          return false;
        }
        return type.IsInterface;
      }

      PropertyInfo[] GetProperties() {
        Type[] interfaces = type_t_.GetInterfaces();
        if (interfaces.Length == 0) {
          return type_t_.GetProperties();
        }

        // The interfaces that was already considered.
        List<Type> considered = new List<Type>(5);
        Queue<Type> queue = new Queue<Type>(5);
        List<PropertyInfo> properties = new List<PropertyInfo>(5*6);
        considered.Add(type_t_);
        queue.Enqueue(type_t_);
        while (queue.Count > 0) {
          Type type = queue.Dequeue();
          foreach (Type t in type.GetInterfaces()) {
            if (!considered.Contains(t)) {
              considered.Add(t);
              queue.Enqueue(t);
            }
          }
          properties.AddRange(type.GetProperties());
        }
        return properties.ToArray();
      }

      void GetMappings(PropertyInfo[] properties, MappingResult result) {
        List<KeyValuePair<string, PropertyInfo>> value_mappings =
          new List<KeyValuePair<string, PropertyInfo>>(properties.Length);
        List<PropertyInfo> reference_mappings =
          new List<PropertyInfo>(properties.Length);
        List<KeyValuePair<ITypeMap, PropertyInfo>> const_mappings =
          new List<KeyValuePair<ITypeMap, PropertyInfo>>(properties.Length);
        for (int i = 0, j = properties.Length; i < j; i++) {
          PropertyInfo property = properties[i];
          ITypeMap mapping;

          // If acustom  map was not defined, maps to the name of the property.
          if (!mappings_.TryGetValue(property.Name, out mapping)) {
            mapping = new StringTypeMap(property.Name);
          }

          if (mapping.MapType == TypeMapType.Ignore) {
            mapping = new IgnoreMapType(GetDefaultValue(property.PropertyType));
            const_mappings
              .Add(new KeyValuePair<ITypeMap, PropertyInfo>(mapping, property));
          } else if (IsReferenceType(property)) {
            reference_mappings.Add(property);
          } else {
            if (mapping.MapType == TypeMapType.String) {
              value_mappings.Add(
                new KeyValuePair<string, PropertyInfo>((string) mapping.Value,
                  property));
            } else {
              const_mappings
                .Add(new KeyValuePair<ITypeMap, PropertyInfo>(mapping, property));
            }
          }
        }
        result.ValueMappings = value_mappings.ToArray();
        result.ReferenceMappings = reference_mappings.ToArray();
        result.ConstantMappings = const_mappings.ToArray();
      }

      object GetDefaultValue(Type type) {
        if (type.IsValueType) {
          return Activator.CreateInstance(type);
        }
        return null;
      }
    }
  }
}
