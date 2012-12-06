using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

namespace Nohros.Data
{
  public abstract partial class DataReaderMapper<T>
  {
    public class Builder
    {
      const string kDynamicAssemblyName = "Nohros.Data.Dynamic";

      static readonly ModuleBuilder module_;
      readonly IDictionary<string, string> mapping_;
      readonly Type type_t_;

      #region .ctor
      static Builder() {
        AppDomain app = AppDomain.CurrentDomain;
        AssemblyName asm_name = new AssemblyName();
        asm_name.Name = kDynamicAssemblyName;
        AssemblyBuilder asm_builder = app
          .DefineDynamicAssembly(asm_name, AssemblyBuilderAccess.Run);
        module_ = asm_builder
          .DefineDynamicModule(asm_builder.GetName().Name, false);
      }
      #endregion

      #region .ctor
      public Builder() {
        if (typeof (T).IsInterface == false) {
          throw new InvalidOperationException(
            string.Format(Resources.Resources.Arg_Type_ShouldBeInterface,
              typeof (T).Name));
        }
        mapping_ = new Dictionary<string, string>();
        type_t_ = typeof (T);
      }
      #endregion

      public Builder Map(string name, string property) {
        mapping_[name] = property;
        return this;
      }

      public IMapper<T> Build() {
      }

      Type GetType() {
        Type type = module_.GetType(GetDynamicTypeName());
        if (type == null) {
          BuildType();
        }
      }

      TypeBuilder BuildType() {
        TypeBuilder builder = module_.DefineType(
          GetDynamicTypeName(),
          TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AutoLayout,
          typeof (DataReaderMapper<T>),
          new Type[] {type_t_, typeof (IMapper<T>)});
        BuildConstructor();
      }

      void BuildConstructor(TypeBuilder type) {
        ConstructorBuilder constructor =
          type.DefineConstructor(MethodAttributes.Public |
            MethodAttributes.HideBySig |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName,
            CallingConventions.Standard, new Type[] {typeof (IDataReader)});

        FieldBuilder ordinals = type
          .DefineField("ordinals_", typeof (int[]), FieldAttributes.Private);

        // Calls the constructor of the base class DataReaderMapper
        ConstructorInfo data_reader_mapper_ctor = typeof (DataReaderMapper<T>)
          .GetConstructor(new Type[] {typeof (IDataReader)});
        ILGenerator il = constructor.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Call, data_reader_mapper_ctor);

        // Get the columns ordinals.
        MethodInfo get_ordinal_method = typeof (IDataReader)
          .GetMethod("GetOrdinal");

        KeyValuePair<string, PropertyInfo>[] field_names = GetMappings();

        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldc_I4_S, field_names.Length);
        il.Emit(OpCodes.Newarr, typeof (int));
        il.Emit(OpCodes.Stfld, ordinals);

        for (int i = 0, j = field_names.Length; i < j; i++) {
          string column_name = field_names[i].Key;
          il.Emit(OpCodes.Ldarg_0);
          il.Emit(OpCodes.Ldfld, ordinals);
          il.Emit(OpCodes.Ldc_I4_S, i);
          il.Emit(OpCodes.Ldarg_1);
          il.Emit(OpCodes.Ldstr, column_name);
          il.Emit(OpCodes.Callvirt, get_ordinal_method);
          il.Emit(OpCodes.Stelem_I4);
        }
        il.Emit(OpCodes.Ret);
      }

      KeyValuePair<string, PropertyInfo>[] GetMappings() {
        List<KeyValuePair<string, PropertyInfo>> mappings =
          new List<KeyValuePair<string, PropertyInfo>>();
        PropertyInfo[] properties = type_t_.GetProperties(BindingFlags.Public);
        for (int i = 0, j = properties.Length; i < j; i++) {
          string mapping;
          PropertyInfo property = properties[i];
          if (!mapping_.TryGetValue(property.Name, out mapping)) {
            mapping = property.Name;
          }
          mappings.Add(new KeyValuePair<string, PropertyInfo>(mapping, property));
        }
        return mappings.ToArray();
      }

      string GetDynamicTypeName() {
        return typeof (T).Name + "_";
      }
    }
  }
}
