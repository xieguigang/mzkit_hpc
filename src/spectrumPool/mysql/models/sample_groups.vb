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
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("sample_groups", Database:="sample_pool", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `sample_groups` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `group_name` VARCHAR(255) NOT NULL,
  `project_id` INT UNSIGNED NOT NULL,
  `sample_files` INT UNSIGNED NOT NULL DEFAULT '0',
  `organism` VARCHAR(255) NOT NULL,
  `bio_samples` VARCHAR(255) NOT NULL,
  `repo_path` VARCHAR(255) NOT NULL,
  `add_time` DATETIME NOT NULL DEFAULT now(),
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  FULLTEXT INDEX `search_sample_info` (`group_name`, `organism`, `bio_samples`) VISIBLE,
  INDEX `project_info_idx` (`project_id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;
")>
Public Class sample_groups: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("group_name"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="group_name")> Public Property group_name As String
    <DatabaseField("project_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="project_id")> Public Property project_id As UInteger
    <DatabaseField("sample_files"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="sample_files")> Public Property sample_files As UInteger
    <DatabaseField("organism"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="organism")> Public Property organism As String
    <DatabaseField("bio_samples"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="bio_samples")> Public Property bio_samples As String
    <DatabaseField("repo_path"), NotNull, DataType(MySqlDbType.VarChar, "255"), Column(Name:="repo_path")> Public Property repo_path As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `sample_groups` (`group_name`, `project_id`, `sample_files`, `organism`, `bio_samples`, `repo_path`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `sample_groups` (`id`, `group_name`, `project_id`, `sample_files`, `organism`, `bio_samples`, `repo_path`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `sample_groups` (`group_name`, `project_id`, `sample_files`, `organism`, `bio_samples`, `repo_path`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `sample_groups` (`id`, `group_name`, `project_id`, `sample_files`, `organism`, `bio_samples`, `repo_path`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `sample_groups` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `sample_groups` SET `id`='{0}', `group_name`='{1}', `project_id`='{2}', `sample_files`='{3}', `organism`='{4}', `bio_samples`='{5}', `repo_path`='{6}', `add_time`='{7}' WHERE `id` = '{8}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `sample_groups` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `sample_groups` (`id`, `group_name`, `project_id`, `sample_files`, `organism`, `bio_samples`, `repo_path`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, group_name, project_id, sample_files, organism, bio_samples, repo_path, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `sample_groups` (`id`, `group_name`, `project_id`, `sample_files`, `organism`, `bio_samples`, `repo_path`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, group_name, project_id, sample_files, organism, bio_samples, repo_path, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, group_name, project_id, sample_files, organism, bio_samples, repo_path, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{group_name}', '{project_id}', '{sample_files}', '{organism}', '{bio_samples}', '{repo_path}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{group_name}', '{project_id}', '{sample_files}', '{organism}', '{bio_samples}', '{repo_path}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `sample_groups` (`id`, `group_name`, `project_id`, `sample_files`, `organism`, `bio_samples`, `repo_path`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, group_name, project_id, sample_files, organism, bio_samples, repo_path, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `sample_groups` (`id`, `group_name`, `project_id`, `sample_files`, `organism`, `bio_samples`, `repo_path`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, group_name, project_id, sample_files, organism, bio_samples, repo_path, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, group_name, project_id, sample_files, organism, bio_samples, repo_path, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `sample_groups` SET `id`='{0}', `group_name`='{1}', `project_id`='{2}', `sample_files`='{3}', `organism`='{4}', `bio_samples`='{5}', `repo_path`='{6}', `add_time`='{7}' WHERE `id` = '{8}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, group_name, project_id, sample_files, organism, bio_samples, repo_path, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As sample_groups
                         Return DirectCast(MyClass.MemberwiseClone, sample_groups)
                     End Function
End Class


End Namespace
