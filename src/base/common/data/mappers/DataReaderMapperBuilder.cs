using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using Nohros.Collections;
using Nohros.Data;
using Nohros.Dynamics;
using PropertyAttributes = System.Reflection.PropertyAttributes;
using ValueMap =
  System.Collections.Generic.KeyValuePair
    <string, System.Reflection.PropertyInfo>;
using ConstantMap =
  System.Collections.Generic.KeyValuePair
    <Nohros.Data.ITypeMap, System.Reflection.PropertyInfo>;
using OrdinalMap =
  System.Collections.Generic.KeyValuePair<int, System.Reflection.PropertyInfo>;

namespace Nohros.Data
{
  internal abstract partial class DataReaderMapper<T>
  {
    public class Builder
    {
      class MappingResult
      {
        public FieldBuilder OrdinalsField { get; set; }
        public OrdinalMap[] OrdinalsMapping { get; set; }
        public FieldInfo ReaderField { get; set; }
        public ValueMap[] ValueMappings { get; set; }
        public ConstantMap[] ConstantMappings { get; set; }
        public FieldInfo LoaderField { get; set; }
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
        if (type_t == null) {
          throw new ArgumentNullException("type_t");
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
        if (source == null) {
          return Map(destination, new IgnoreMapType());
        }
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

      /// <summary>
      /// Maps the constant <see cref="value"/> to the interface property
      /// <paramref source="destination"/>.
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
      public Builder Map(string destination, ITypeMap value) {
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
        if (reader == null) {
          throw new ArgumentNullException("reader");
        }
        return Build(reader, null);
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
      public DataReaderMapper<T> Build(IDataReader reader,
        CallableDelegate<T> loader) {
        DataReaderMapper<T> mapper =
          (DataReaderMapper<T>) Activator.CreateInstance(GetDynamicType());
        mapper.Initialize(reader, loader);
        return mapper;
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
      internal Type GetDynamicType() {
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
        mappings_.Add(destination, new IgnoreMapType());
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
          new Type[] {
            typeof (IMapper<T>), typeof (IForwardOnlyEnumerable<T>)
          });

        PropertyInfo[] properties = GetProperties();

        MappingResult result = new MappingResult();

        // Get the mappings for the properties that return value types.
        GetMappings(properties, result);

        EmitConstructor(builder, result);
        EmitGetOrdinals(builder, result);
        EmitNewT(builder, result);
        EmitMapMethod(builder, result);

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
            CallingConventions.Standard, Type.EmptyTypes);

        // Calls the constructor of the base class DataReaderMapper
        ConstructorInfo data_reader_mapper_ctor = typeof (DataReaderMapper<T>)
          .GetConstructor(
            BindingFlags.Public |
              BindingFlags.NonPublic |
              BindingFlags.Instance,
            null, Type.EmptyTypes, null);
        ILGenerator il = constructor.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0); // load "this" pointer
        il.Emit(OpCodes.Call, data_reader_mapper_ctor); // call default ctor

        // Create the array that will store the column ordinals.
        result.OrdinalsField = type
          .DefineField("ordinals_", typeof (int[]), FieldAttributes.Private);

        result.ReaderField = typeof (DataReaderMapper<T>)
          .GetField("reader_", BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance);

        result.LoaderField = typeof (DataReaderMapper<T>)
          .GetField("loader_", BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance);

        // return from the constructor
        il.Emit(OpCodes.Ret);
      }

      void EmitNewT(TypeBuilder type, MappingResult result) {
        MethodBuilder builder = type
          .DefineMethod("NewT",
            MethodAttributes.Assembly | MethodAttributes.HideBySig |
              MethodAttributes.Virtual, type_t_, Type.EmptyTypes);

        ILGenerator il = builder.GetILGenerator();

        // Calls the default constructor of the type T
        ConstructorInfo t_constructor = type_t_
          .GetConstructor(
            BindingFlags.Public |
              BindingFlags.NonPublic | BindingFlags.Instance,
            null, Type.EmptyTypes, null);

        if (t_constructor == null) {
          throw new MissingMethodException(type_t_.Name, "ctor()");
        }

        LocalBuilder local_t = il.DeclareLocal(type_t_);
        il.Emit(OpCodes.Newobj, t_constructor);
        il.Emit(OpCodes.Stloc_0, local_t);
        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ret);
      }

      void EmitGetOrdinals(TypeBuilder type, MappingResult result) {
        MethodBuilder builder = type
          .DefineMethod("GetOrdinals",
            MethodAttributes.Assembly | MethodAttributes.HideBySig |
              MethodAttributes.Virtual, typeof (void), Type.EmptyTypes);

        ILGenerator il = builder.GetILGenerator();

        // If there is nothing to be mapped, map nothing.
        if (result.ValueMappings.Length == 0) {
          // Create an empty array to prevent the NoResultException to be throw
          // by the Map method.
          il.Emit(OpCodes.Ldarg_0);
          il.Emit(OpCodes.Ldc_I4_0);
          il.Emit(OpCodes.Newarr, typeof (int));
          il.Emit(OpCodes.Stfld, result.OrdinalsField);

          result.OrdinalsMapping = new KeyValuePair<int, PropertyInfo>[0];
        } else {
          // get the columns ordinals, using a try/catch block to prevent a
          // InvalidOperationException to be throw when no recordset is returned.
          il.BeginExceptionBlock();

          result.OrdinalsMapping = EmitOrdinals(il, result);

          //il.Emit(OpCodes.Leave, exit_try_label);
          il.BeginCatchBlock(typeof (InvalidOperationException));

          // remove the exception from the stack
          il.Emit(OpCodes.Pop);

          // set [ordinals_] to null
          il.Emit(OpCodes.Ldarg_0);
          il.Emit(OpCodes.Ldnull);
          il.Emit(OpCodes.Stfld, result.OrdinalsField);
          il.EndExceptionBlock();
        }

        MethodInfo method_info =
          typeof (DataReaderMapper<T>).GetMethod("Initialize",
            BindingFlags.NonPublic | BindingFlags.Public |
              BindingFlags.Instance,
            null, new Type[] {typeof (IDataReader)}, null);

        il.Emit(OpCodes.Ret);
      }

      void EmitMapMethod(TypeBuilder type, MappingResult result) {
        MethodBuilder builder = type
          .DefineMethod("Map",
            MethodAttributes.Public | MethodAttributes.HideBySig |
              MethodAttributes.Virtual, typeof (T), Type.EmptyTypes);

        ILGenerator il = builder.GetILGenerator();

        // Create a new instance of the T using the associated class loader and
        // stores in a local variable.
        il.DeclareLocal(type_t_);
        MethodInfo callable = typeof (CallableDelegate<T>).GetMethod("Invoke");
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, result.LoaderField);
        il.Emit(OpCodes.Callvirt, callable);
        il.Emit(OpCodes.Stloc_0);

        // Set the value of the properties of the newly created T object.
        OrdinalMap[] fields = result.OrdinalsMapping;
        for (int i = 0, j = fields.Length; i < j; i++) {
          int ordinal = fields[i].Key;
          PropertyInfo property = fields[i].Value;

          MethodInfo get_x_method =
            Dynamics_.GetDataReaderMethod(
              Dynamics_.GetDataReaderMethodName(property.PropertyType));

          // Get the set method of the current property. If the property does
          // not have a set method ignores it.
          MethodInfo set_x_property = property.GetSetMethod(true);
          if (set_x_property == null) {
            continue;
          }

          il.Emit(OpCodes.Ldloc_0);
          il.Emit(OpCodes.Ldarg_0);
          il.Emit(OpCodes.Ldfld, result.ReaderField);
          il.Emit(OpCodes.Ldarg_0);
          il.Emit(OpCodes.Ldfld, result.OrdinalsField);
          EmitLoad(il, ordinal);
          il.Emit(OpCodes.Ldelem_I4);
          il.Emit(OpCodes.Callvirt, get_x_method);
          il.Emit(OpCodes.Callvirt, set_x_property);
        }

        ConstantMap[] constant_maps = result.ConstantMappings;
        for (int i = 0, j = constant_maps.Length; i < j; i++) {
          ITypeMap map = constant_maps[i].Key;
          PropertyInfo property = constant_maps[i].Value;
          if (map.MapType != TypeMapType.Ignore) {
            // Get the set method of the current property. If the property does
            // not have a set method ignores it.
            MethodInfo set_x_property = property.GetSetMethod(true);
            if (set_x_property == null) {
              continue;
            }

            il.Emit(OpCodes.Ldloc_0);
            EmitLoad(il, map);
            il.Emit(OpCodes.Callvirt, set_x_property);
          }
        }

        // load the local T and return.
        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ret);
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

      void EmitLoad(ILGenerator il, int value) {
        if (value > -1 && value < 9) {
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
        } else if (value > -128 && value < 128) {
          il.Emit(OpCodes.Ldc_I4_S, value);
        } else {
          il.Emit(OpCodes.Ldc_I4, value);
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
        MappingResult result) {
        KeyValuePair<string, PropertyInfo>[] fields = result.ValueMappings;
        KeyValuePair<int, PropertyInfo>[] ordinals_mapping =
          new KeyValuePair<int, PropertyInfo>[fields.Length];

        if (fields.Length > 0) {
          MethodInfo get_ordinal_method =
            Dynamics_.GetDataReaderMethod("GetOrdinal");

          il.Emit(OpCodes.Ldarg_0);
          il.Emit(OpCodes.Ldc_I4, fields.Length);
          il.Emit(OpCodes.Newarr, typeof (int));
          il.Emit(OpCodes.Stfld, result.OrdinalsField);

          for (int i = 0, j = fields.Length; i < j; i++) {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, result.OrdinalsField);
            il.Emit(OpCodes.Ldc_I4, i);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, result.ReaderField);
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
        return true;
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
        List<ValueMap> value_mappings =
          new List<ValueMap>(properties.Length);
        List<ConstantMap> const_mappings =
          new List<ConstantMap>(properties.Length);
        for (int i = 0, j = properties.Length; i < j; i++) {
          PropertyInfo property = properties[i];
          ITypeMap mapping;

          // If acustom  map was not defined, maps to the name of the property.
          if (!mappings_.TryGetValue(property.Name, out mapping)) {
            mapping = new StringTypeMap(property.Name);
          }

          if (mapping.MapType == TypeMapType.Ignore || IsReferenceType(property)) {
            mapping = new IgnoreMapType(GetDefaultValue(property.PropertyType));
            const_mappings.Add(new ConstantMap(mapping, property));
          } else if (mapping.MapType == TypeMapType.String) {
            value_mappings.Add(new ValueMap((string) mapping.Value, property));
          } else {
            const_mappings.Add(new ConstantMap(mapping, property));
          }
        }
        result.ValueMappings = value_mappings.ToArray();
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
