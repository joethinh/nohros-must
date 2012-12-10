using System;
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
      }

      readonly Type type_t_;
      readonly IDictionary<string, string> value_mapping_;

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
        value_mapping_ = new Dictionary<string, string>(
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
      /// between source columns and destination properties
      /// </param>
      /// <remarks>
      /// The <see cref="KeyValuePair{TKey,TValue}.Key"/> is used as the source
      /// column name and the <see cref="KeyValuePair{TKey,TValue}.Value"/> is
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
      /// between source columns and destination properties
      /// </param>
      /// <remarks>
      /// The <see cref="KeyValuePair{TKey,TValue}.Key"/> is used as the source
      /// column name and the <see cref="KeyValuePair{TKey,TValue}.Value"/> is
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
      public Builder Map(string source, string destination) {
        value_mapping_[destination] = source;
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

        PropertyInfo[] properties = type_t_.GetProperties();

        // Get the mappings for the properties that return value types.
        KeyValuePair<string, PropertyInfo>[] value_mappings =
          GetValueMappings(properties);

        // Get the mappings for the properties that return value types.
        PropertyInfo[] reference_fields = GetReferenceMappings(properties);
        KeyValuePair<FieldBuilder, PropertyInfo>[] reference_mappings =
          EmitReferenceFields(builder, reference_fields);

        MappingResult result = EmitConstructor(builder, reference_mappings,
          value_mappings);
        EmitMapMethod(builder);
        EmitReferenceProperties(builder, reference_mappings);
        EmitValueProperties(builder, result.OrdinalsField, result.ReaderField,
          result.OrdinalsMapping);
        return builder.CreateType();
      }

      /// <summary>
      /// Emit the constuctor code.
      /// </summary>
      MappingResult EmitConstructor(TypeBuilder type,
        KeyValuePair<FieldBuilder, PropertyInfo>[] reference_mappings,
        KeyValuePair<string, PropertyInfo>[] value_mappings) {
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

        MappingResult result = new MappingResult();

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
          EmitOrdinals(il, result.OrdinalsField, value_mappings);

        // create the reference objects.
        EmitReferencesInitialization(il, reference_mappings);

        // return from the constructor
        il.Emit(OpCodes.Ret);

        return result;
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
            .InvokeMember("GetDynamicType", BindingFlags.InvokeMethod, null,
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

      KeyValuePair<int, PropertyInfo>[] EmitOrdinals(ILGenerator il,
        FieldBuilder ordinals, KeyValuePair<string, PropertyInfo>[] fields) {
        KeyValuePair<int, PropertyInfo>[] ordinals_mapping =
          new KeyValuePair<int, PropertyInfo>[fields.Length];
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
        return ordinals_mapping;
      }

      string GetMemberName(string name) {
        return name.ToLower() + "_";
      }

      PropertyInfo[] GetReferenceMappings(PropertyInfo[] properties) {
        List<PropertyInfo> mappings = new List<PropertyInfo>(properties.Length);
        for (int i = 0, j = properties.Length; i < j; i++) {
          PropertyInfo property = properties[i];
          if (IsReferenceType(property)) {
            mappings.Add(property);
          }
        }
        return mappings.ToArray();
      }

      bool IsReferenceType(PropertyInfo property) {
        Type type = property.PropertyType;
        if (type.IsValueType || type.Name == "String") {
          return false;
        }
        return type.IsInterface;
      }

      KeyValuePair<string, PropertyInfo>[] GetValueMappings(
        PropertyInfo[] properties) {
        List<KeyValuePair<string, PropertyInfo>> mappings =
          new List<KeyValuePair<string, PropertyInfo>>();
        for (int i = 0, j = properties.Length; i < j; i++) {
          PropertyInfo property = properties[i];
          if (!IsReferenceType(property)) {
            string mapping;
            if (!value_mapping_.TryGetValue(property.Name, out mapping)) {
              mapping = property.Name;
            }
            mappings
              .Add(new KeyValuePair<string, PropertyInfo>(mapping, property));
          }
        }
        return mappings.ToArray();
      }
    }
  }
}
