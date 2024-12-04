REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @12/4/2024 03:28:07 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace treeModel

''' <summary>
''' ```SQL
''' atom group information data
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("atoms", Database:="molecule_tree", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `atoms` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'the unique reference id of current atom group, in integer id representative',
  `unique_id` VARCHAR(64) NOT NULL COMMENT 'the unique reference id of current atom group, in character representative',
  `atom_group` VARCHAR(45) NOT NULL COMMENT 'the atom group name, could be duplicated',
  `element` VARCHAR(8) NOT NULL COMMENT 'the base element atom of current group data',
  `aromatic` INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'on an aromatic ring? 1 means true',
  `hydrogen` INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'The number of the hydrogen of current atom group it has',
  `charge` INT NOT NULL DEFAULT 0 COMMENT 'the ion charge value of current atom group, could be a positive/negative integer value',
  `add_time` DATETIME NOT NULL DEFAULT now() COMMENT 'create time of this atom group',
  `note` LONGTEXT NULL COMMENT 'description about this atom group data',
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'atom group information data';
")>
Public Class atoms: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
''' <summary>
''' the unique reference id of current atom group, in integer id representative
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the unique reference id of current atom group, in character representative
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("unique_id"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="unique_id")> Public Property unique_id As String
''' <summary>
''' the atom group name, could be duplicated
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("atom_group"), NotNull, DataType(MySqlDbType.VarChar, "45"), Column(Name:="atom_group")> Public Property atom_group As String
''' <summary>
''' the base element atom of current group data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("element"), NotNull, DataType(MySqlDbType.VarChar, "8"), Column(Name:="element")> Public Property element As String
''' <summary>
''' on an aromatic ring? 1 means true
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("aromatic"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="aromatic")> Public Property aromatic As UInteger
''' <summary>
''' The number of the hydrogen of current atom group it has
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hydrogen"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="hydrogen")> Public Property hydrogen As UInteger
''' <summary>
''' the ion charge value of current atom group, could be a positive/negative integer value
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("charge"), NotNull, DataType(MySqlDbType.Int32, "11"), Column(Name:="charge")> Public Property charge As Integer
''' <summary>
''' create time of this atom group
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
''' <summary>
''' description about this atom group data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("note"), DataType(MySqlDbType.Text), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `atoms` (`unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `atoms` (`unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `atoms` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `atoms` SET `id`='{0}', `unique_id`='{1}', `atom_group`='{2}', `element`='{3}', `aromatic`='{4}', `hydrogen`='{5}', `charge`='{6}', `add_time`='{7}', `note`='{8}' WHERE `id` = '{9}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `atoms` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{unique_id}', '{atom_group}', '{element}', '{aromatic}', '{hydrogen}', '{charge}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{unique_id}', '{atom_group}', '{element}', '{aromatic}', '{hydrogen}', '{charge}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `atoms` (`id`, `unique_id`, `atom_group`, `element`, `aromatic`, `hydrogen`, `charge`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `atoms` SET `id`='{0}', `unique_id`='{1}', `atom_group`='{2}', `element`='{3}', `aromatic`='{4}', `hydrogen`='{5}', `charge`='{6}', `add_time`='{7}', `note`='{8}' WHERE `id` = '{9}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, unique_id, atom_group, element, aromatic, hydrogen, charge, MySqlScript.ToMySqlDateTimeString(add_time), note, id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As atoms
                         Return DirectCast(MyClass.MemberwiseClone, atoms)
                     End Function
End Class


End Namespace
