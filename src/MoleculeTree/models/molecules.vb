REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @12/4/2024 03:45:32 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace treeModel

''' <summary>
''' ```SQL
''' cache pool of external molecule data set
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("molecules", Database:="molecule_tree", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `molecules` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `db_xref` VARCHAR(255) NOT NULL,
  `name` VARCHAR(2048) NOT NULL,
  `formula` VARCHAR(64) NOT NULL,
  `exact_mass` DOUBLE UNSIGNED NOT NULL DEFAULT 0,
  `smiles` LONGTEXT NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'cache pool of external molecule data set';
")>
Public Class molecules: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("db_xref"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="db_xref")> Public Property db_xref As String
    <DatabaseField("name"), NotNull, DataType(MySqlDbType.VarChar, "2048"), Column(Name:="name")> Public Property name As String
    <DatabaseField("formula"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="formula")> Public Property formula As String
    <DatabaseField("exact_mass"), NotNull, DataType(MySqlDbType.Double), Column(Name:="exact_mass")> Public Property exact_mass As Double
    <DatabaseField("smiles"), NotNull, DataType(MySqlDbType.Text), Column(Name:="smiles")> Public Property smiles As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `molecules` (`db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `molecules` (`db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `molecules` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `molecules` SET `id`='{0}', `db_xref`='{1}', `name`='{2}', `formula`='{3}', `exact_mass`='{4}', `smiles`='{5}', `add_time`='{6}' WHERE `id` = '{7}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `molecules` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{db_xref}', '{name}', '{formula}', '{exact_mass}', '{smiles}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{db_xref}', '{name}', '{formula}', '{exact_mass}', '{smiles}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `molecules` (`id`, `db_xref`, `name`, `formula`, `exact_mass`, `smiles`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `molecules` SET `id`='{0}', `db_xref`='{1}', `name`='{2}', `formula`='{3}', `exact_mass`='{4}', `smiles`='{5}', `add_time`='{6}' WHERE `id` = '{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, db_xref, name, formula, exact_mass, smiles, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As molecules
                         Return DirectCast(MyClass.MemberwiseClone, molecules)
                     End Function
End Class


End Namespace
