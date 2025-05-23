REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @5/7/2025 10:20:56 AM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace clusterModels

''' <summary>
''' ```SQL
''' the spectrum data that extract from a cluster data
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("consensus_spectrum", Database:="sample_pool", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `consensus_spectrum` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `cluster_id` INT UNSIGNED NOT NULL,
  `parameter_id` INT UNSIGNED NOT NULL COMMENT 'parameter id reference of cutoff range and umap parameters',
  `spectrum` VARCHAR(4096) NOT NULL COMMENT 'consensus peak and intensity value, base64strng encoded of double number in network byteorder',
  `peak_ranking` VARCHAR(4096) NOT NULL COMMENT 'consensus peak and ranking value, base64 string encoded double numbers in network byteorder',
  `umap` VARCHAR(2048) NOT NULL COMMENT 'base64string for umap result of multiple dimension number encoded in network byte order',
  `add_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `cluster_info_idx` (`cluster_id` ASC) VISIBLE,
  INDEX `parameter_info_idx` (`parameter_id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3
COMMENT = 'the spectrum data that extract from a cluster data';
")>
Public Class consensus_spectrum: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("cluster_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="cluster_id")> Public Property cluster_id As UInteger
''' <summary>
''' parameter id reference of cutoff range and umap parameters
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("parameter_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="parameter_id")> Public Property parameter_id As UInteger
''' <summary>
''' consensus peak and intensity value, base64strng encoded of double number in network byteorder
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("spectrum"), NotNull, DataType(MySqlDbType.VarChar, "4096"), Column(Name:="spectrum")> Public Property spectrum As String
''' <summary>
''' consensus peak and ranking value, base64 string encoded double numbers in network byteorder
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("peak_ranking"), NotNull, DataType(MySqlDbType.VarChar, "4096"), Column(Name:="peak_ranking")> Public Property peak_ranking As String
''' <summary>
''' base64string for umap result of multiple dimension number encoded in network byte order
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("umap"), NotNull, DataType(MySqlDbType.VarChar, "2048"), Column(Name:="umap")> Public Property umap As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `consensus_spectrum` (`cluster_id`, `parameter_id`, `spectrum`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `spectrum`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `consensus_spectrum` (`cluster_id`, `parameter_id`, `spectrum`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `spectrum`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `consensus_spectrum` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `consensus_spectrum` SET `id`='{0}', `cluster_id`='{1}', `parameter_id`='{2}', `spectrum`='{3}', `peak_ranking`='{4}', `umap`='{5}', `add_time`='{6}' WHERE `id` = '{7}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `consensus_spectrum` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `spectrum`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, cluster_id, parameter_id, spectrum, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `spectrum`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, cluster_id, parameter_id, spectrum, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, cluster_id, parameter_id, spectrum, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{cluster_id}', '{parameter_id}', '{spectrum}', '{peak_ranking}', '{umap}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{cluster_id}', '{parameter_id}', '{spectrum}', '{peak_ranking}', '{umap}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `spectrum`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, cluster_id, parameter_id, spectrum, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `consensus_spectrum` (`id`, `cluster_id`, `parameter_id`, `spectrum`, `peak_ranking`, `umap`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, cluster_id, parameter_id, spectrum, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, cluster_id, parameter_id, spectrum, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `consensus_spectrum` SET `id`='{0}', `cluster_id`='{1}', `parameter_id`='{2}', `spectrum`='{3}', `peak_ranking`='{4}', `umap`='{5}', `add_time`='{6}' WHERE `id` = '{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, cluster_id, parameter_id, spectrum, peak_ranking, umap, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As consensus_spectrum
                         Return DirectCast(MyClass.MemberwiseClone, consensus_spectrum)
                     End Function
End Class


End Namespace
