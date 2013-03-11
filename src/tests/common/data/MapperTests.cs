using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using Nohros.Data;
using Nohros.Dynamics;
using Telerik.JustMock;

namespace Nohros.Common
{
  public class MapperTests
  {
    public class KeyedMapperTest
    {
      public string Name { get; set; }
    }

    public class MapperDerivedTest : MapperTest
    {
    }

    public class MapperTest
    {
      public string Name { get; set; }
    }

    public class IgnoreMapperTest
    {
      public string Name { get; set; }
    }

    public class NestedMapperTest
    {
      public MapperTest Nested { get; set; }
    }

    public class MapNestedMapperTest
    {
      public MapperTest Nested { get; set; }
    }

    public class MapMapperTest
    {
      public string Name { get; set; }
    }

    [Test]
    public void ShouldBuildDynamicType() {
      var reader = Mock.Create<IDataReader>();
      var mapper = new DataReaderMapper<MapperTest>.Builder()
        .Map("usuario_nome", "name")
        .Build(reader);
      Dynamics_.AssemblyBuilder.Save("test.dll");
      Assert.That(mapper, Is.AssignableTo<DataReaderMapper<MapperTest>>());
      Assert.That(mapper, Is.AssignableTo<MapperTest>());
    }

    [Test]
    public void ShouldBuildNestedDynamicType() {
      var reader = Mock.Create<IDataReader>();
      Mock
        .Arrange(() => reader.GetOrdinal(Arg.AnyString))
        .Returns(0);
      var mapper = new DataReaderMapper<NestedMapperTest>.Builder()
        .Map("usuario_nome", "name")
        .Build(reader);
      var inner = mapper.Map().Nested;
      Assert.That(inner, Is.AssignableTo<DataReaderMapper<MapperTest>>());
      Assert.That(inner, Is.AssignableTo<MapperTest>());
      Assert.That(mapper, Is.AssignableTo<DataReaderMapper<NestedMapperTest>>());
      Assert.That(mapper, Is.AssignableTo<NestedMapperTest>());
    }

    public interface ICrmEvent
    {
      /// <summary>
      /// Retorna um numero que identifica o acionamento de forma unica em
      /// um servidor.
      /// </summary>
      /// <remarks>
      /// Este valor esta, geralmente, associado ao ID do registro que contem
      /// os dados do acinamento.
      /// </remarks>
      int ID { get; }

      /// <summary>
      /// Retorna um numero que identifica o tipo de acionamento realizado.
      /// </summary>
      int TypeID { get; }

      /// <summary>
      /// Retorna o codigo do agente que realizou o acionamento.
      /// </summary>
      int AgentID { get; }

      /// <summary>
      /// Retorna um numero que identifica o contato acionado de forma unica em
      /// um servidor.
      /// </summary>
      int ContactID { get; }

      /// <summary>
      /// Retorna a data em que o acionamento foi realizado.
      /// </summary>
      DateTime Date { get; }

      Guid ServerID { get; set; }
    }

    public class CrmEvent : ICrmEvent
    {
      /// <inheritdoc/>
      public int ID { get; set; }

      /// <inheritdoc/>
      public int TypeID { get; set; }

      /// <inheritdoc/>
      public int AgentID { get; set; }

      /// <inheritdoc/>
      public int ContactID { get; set; }

      /// <inheritdoc/>
      public DateTime Date { get; set; }

      /// <inheritdoc/>
      public Guid ServerID { get; set; }
    }

    [Test]
    public void GetDynamicType() {
      /*var builder = new DataReaderMapper<NestedMapperTest>.Builder()
        .Map("usuario_nome", "name");
      Type type = builder.GetDynamicType();
      Dynamics_.AssemblyBuilder.Save("test.dll");*/
      var reader = Mock.Create<IDataReader>();
      Mappers.GetMapper<CrmEvent>(reader,
        () => new[] {
          new KeyValuePair<string, string>("id", "cod_hist_cli"),
          new KeyValuePair<string, string>("typeid","cod_ocor"),
          new KeyValuePair<string, string>("agentid","usuario_cad"),
          new KeyValuePair<string, string>("contactid","cod_dev"),
          new KeyValuePair<string, string>("date", "data_cad"),
          new KeyValuePair<string, string>("serverid", null)
        },()=>new CrmEvent {
          ServerID = Guid.NewGuid()
        });
      Dynamics_.AssemblyBuilder.Save("test.dll");
    }

    [Test]
    public void ShouldBuildDerivedInterface() {
      var reader = Mock.Create<IDataReader>();
      var mapper = new DataReaderMapper<MapperDerivedTest>.Builder()
        .Build(reader);
      Assert.That(mapper,
        Is.AssignableTo<DataReaderMapper<MapperDerivedTest>>());
      Assert.That(mapper, Is.AssignableTo<MapperDerivedTest>());
      Assert.That(mapper, Is.AssignableTo<MapperTest>());
    }

    [Test]
    public void ShouldIgnoreProperty() {
      var reader = Mock.Create<IDataReader>();
      var mapper = new DataReaderMapper<IgnoreMapperTest>.Builder()
        .Ignore("name")
        .Build(reader);
      Assert.That(() => mapper.Map().Name,
        Throws.TypeOf<NotImplementedException>());
    }

    [Test]
    public void ShouldMapCustomColumnToProperty() {
      var builder = new SqlConnectionStringBuilder();
      builder.DataSource = "192.168.203.186";
      builder.UserID = "nohros";
      builder.Password = "Noors03";

      using (var conn = new SqlConnection(builder.ToString()))
      using (var cmd = new SqlCommand("select 'nohros' as usuario_nome", conn)) {
        conn.Open();
        using (var reader = cmd.ExecuteReader()) {
          var mapper = new DataReaderMapper<MapMapperTest>.Builder()
            .Map("name", "usuario_nome")
            .Build(reader);
          Assert.That(mapper.Map().Name, Is.EqualTo("nohros"));
        }
      }
    }

    [Test]
    public void ShouldMapArrayOfKeyValuePairs() {
      var builder = new SqlConnectionStringBuilder();
      builder.DataSource = "192.168.203.186";
      builder.UserID = "nohros";
      builder.Password = "Noors03";

      using (var conn = new SqlConnection(builder.ToString()))
      using (var cmd = new SqlCommand("select 'nohros' as usuario_nome", conn)) {
        conn.Open();
        using (var reader = cmd.ExecuteReader()) {
          var mapper = new DataReaderMapper<KeyedMapperTest>.Builder(
            new KeyValuePair<string, string>[] {
              new KeyValuePair<string, string>("name", "usuario_nome")
            })
            .Build(reader);
          Assert.That(mapper.Map().Name, Is.EqualTo("nohros"));
        }
      }
    }

    [Test]
    public void ShouldMapNestedInterface() {
      var builder = new SqlConnectionStringBuilder();
      builder.DataSource = "192.168.203.186";
      builder.UserID = "nohros";
      builder.Password = "Noors03";

      using (var conn = new SqlConnection(builder.ToString()))
      using (var cmd = new SqlCommand("select 'nohros' as usuario_nome", conn)) {
        conn.Open();
        using (var reader = cmd.ExecuteReader()) {
          var derived = new DataReaderMapper<MapNestedMapperTest>.Builder()
            .Build(reader, ()=> {
              return new MapNestedMapperTest {
                Nested = new DataReaderMapper<MapperTest>.Builder()
                  .Map("name", "usuario_nome")
                  .Build(reader)
                  .Map()
              };
            });
          reader.Read();
          Assert.That(derived.Map().Nested.Name, Is.EqualTo("nohros"));
        }
      }
    }

    [Test]
    public void ShoudMapToConstValues() {
      var reader = Mock.Create<IDataReader>();
      var mapper = new DataReaderMapper<IgnoreMapperTest>.Builder()
        .Map("name", new ConstStringMapType("myname"))
        .Build(reader);
      Dynamics_.AssemblyBuilder.Save("test.dll");
      Assert.That(mapper.Map().Name, Is.EqualTo("myname"));
    }
  }
}
