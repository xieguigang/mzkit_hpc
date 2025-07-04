#Region "Microsoft.VisualBasic::b52ef7ee64811eafb87ad3be8549a845, Rscript\Library\mzkit_hpc\src\spectrumPool\mysql\models\project.vb"

    ' Author:
    ' 
    '       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
    ' 
    ' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 178
    '    Code Lines: 90 (50.56%)
    ' Comment Lines: 66 (37.08%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 22 (12.36%)
    '     File Size: 8.53 KB


    ' Class project
    ' 
    '     Properties: add_time, id, note, project_id, project_name
    '                 sample_files, sample_groups
    ' 
    '     Function: Clone, GetDeleteSQL, GetDumpInsertValue, (+2 Overloads) GetInsertSQL, (+2 Overloads) GetReplaceSQL
    '               GetUpdateSQL
    ' 
    ' 
    ' /********************************************************************************/

#End Region

REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @5/28/2025 02:55:31 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace clusterModels

''' <summary>
''' ```SQL
''' the sample file rawdata source note
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("project", Database:="sample_pool", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `project` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `project_id` VARCHAR(128) NOT NULL COMMENT 'the unique identifier of this project in character mode',
  `project_name` VARCHAR(1024) NOT NULL COMMENT 'the project display name',
  `sample_groups` INT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'number of the sample groups of the samples files in this project',
  `sample_files` INT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'number of the sample files in current project',
  `add_time` DATETIME NOT NULL DEFAULT now(),
  `note` LONGTEXT NULL,
  PRIMARY KEY (`id`, `project_id`),
  UNIQUE INDEX `idproject_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `search_id` (`project_id` ASC) VISIBLE,
  FULLTEXT INDEX `search_name` (`project_name`, `note`) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3
COMMENT = 'the sample file rawdata source note';
")>
Public Class project: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id")> Public Property id As UInteger
''' <summary>
''' the unique identifier of this project in character mode
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("project_id"), NotNull, DataType(MySqlDbType.VarChar, "128"), Column(Name:="project_id")> Public Property project_id As String
''' <summary>
''' the project display name
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("project_name"), NotNull, DataType(MySqlDbType.VarChar, "1024"), Column(Name:="project_name")> Public Property project_name As String
''' <summary>
''' number of the sample groups of the samples files in this project
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("sample_groups"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="sample_groups")> Public Property sample_groups As UInteger
''' <summary>
''' number of the sample files in current project
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("sample_files"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="sample_files")> Public Property sample_files As UInteger
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
    <DatabaseField("note"), DataType(MySqlDbType.Text), Column(Name:="note")> Public Property note As String
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `project` (`project_id`, `project_name`, `sample_groups`, `sample_files`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `project` (`id`, `project_id`, `project_name`, `sample_groups`, `sample_files`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `project` (`project_id`, `project_name`, `sample_groups`, `sample_files`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `project` (`id`, `project_id`, `project_name`, `sample_groups`, `sample_files`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `project` WHERE `id`, `project_id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `project` SET `id`='{0}', `project_id`='{1}', `project_name`='{2}', `sample_groups`='{3}', `sample_files`='{4}', `add_time`='{5}', `note`='{6}' WHERE `id`, `project_id` = '{7}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `project` WHERE `id`, `project_id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id, project_id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `project` (`id`, `project_id`, `project_name`, `sample_groups`, `sample_files`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, project_id, project_name, sample_groups, sample_files, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `project` (`id`, `project_id`, `project_name`, `sample_groups`, `sample_files`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, project_id, project_name, sample_groups, sample_files, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(INSERT_SQL, project_id, project_name, sample_groups, sample_files, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{project_id}', '{project_name}', '{sample_groups}', '{sample_files}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        Else
            Return $"('{project_id}', '{project_name}', '{sample_groups}', '{sample_files}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}', '{note}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `project` (`id`, `project_id`, `project_name`, `sample_groups`, `sample_files`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, project_id, project_name, sample_groups, sample_files, MySqlScript.ToMySqlDateTimeString(add_time), note)
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `project` (`id`, `project_id`, `project_name`, `sample_groups`, `sample_files`, `add_time`, `note`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, project_id, project_name, sample_groups, sample_files, MySqlScript.ToMySqlDateTimeString(add_time), note)
        Else
        Return String.Format(REPLACE_SQL, project_id, project_name, sample_groups, sample_files, MySqlScript.ToMySqlDateTimeString(add_time), note)
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `project` SET `id`='{0}', `project_id`='{1}', `project_name`='{2}', `sample_groups`='{3}', `sample_files`='{4}', `add_time`='{5}', `note`='{6}' WHERE `id`, `project_id` = '{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, project_id, project_name, sample_groups, sample_files, MySqlScript.ToMySqlDateTimeString(add_time), note, id, project_id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As project
                         Return DirectCast(MyClass.MemberwiseClone, project)
                     End Function
End Class


End Namespace
