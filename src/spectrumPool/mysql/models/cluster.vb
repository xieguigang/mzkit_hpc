#Region "Microsoft.VisualBasic::475f51beff3be2d274433a6405841a41, Rscript\Library\mzkit_hpc\src\spectrumPool\mysql\models\cluster.vb"

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

    '   Total Lines: 221
    '    Code Lines: 104 (47.06%)
    ' Comment Lines: 95 (42.99%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 22 (9.95%)
    '     File Size: 11.92 KB


    ' Class cluster
    ' 
    '     Properties: add_time, annotations, consensus, depth, hash_index
    '                 id, key, model_id, n_childs, n_spectrum
    '                 parent_id, root
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
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("cluster", Database:="sample_pool", SchemaSQL:="
CREATE TABLE IF NOT EXISTS `cluster` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `model_id` INT UNSIGNED NOT NULL COMMENT 'the graph model reference id',
  `key` VARCHAR(64) NOT NULL COMMENT 'the key name of current cluster tree node',
  `parent_id` INT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'default value 0 means current tree node is root node',
  `n_childs` INT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'the number of childs that contains in current cluster tree node',
  `n_spectrum` INT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'the cluster size: number of the spectrum data that inside this cluster data',
  `root` INT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'the unique id of the root reference spectrum data for current cluster tree node',
  `hash_index` CHAR(32) NOT NULL COMMENT 'the md5 hash key of the tree pathfor search a specific tree node quickly',
  `depth` INT UNSIGNED NOT NULL DEFAULT '1' COMMENT 'the tree path depth of current cluster node',
  `consensus` VARCHAR(4096) NULL DEFAULT '*',
  `annotations` MEDIUMTEXT NULL DEFAULT NULL COMMENT 'the annotation of the current spectrum cluster data',
  `add_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `spectrum_index` (`root` ASC) INVISIBLE,
  INDEX `parent_node` (`parent_id` ASC) INVISIBLE,
  INDEX `hash_key` (`hash_index` ASC) INVISIBLE,
  INDEX `model_index` (`model_id` ASC) VISIBLE,
  INDEX `cluster_index` (`hash_index` ASC, `model_id` ASC) VISIBLE,
  INDEX `spectrum_size_index` (`n_spectrum` ASC) INVISIBLE,
  INDEX `child_size_index` (`n_childs` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;
")>
Public Class cluster: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the graph model reference id
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("model_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="model_id")> Public Property model_id As UInteger
''' <summary>
''' the key name of current cluster tree node
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("key"), NotNull, DataType(MySqlDbType.VarChar, "64"), Column(Name:="key")> Public Property key As String
''' <summary>
''' default value 0 means current tree node is root node
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("parent_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="parent_id")> Public Property parent_id As UInteger
''' <summary>
''' the number of childs that contains in current cluster tree node
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("n_childs"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="n_childs")> Public Property n_childs As UInteger
''' <summary>
''' the cluster size: number of the spectrum data that inside this cluster data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("n_spectrum"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="n_spectrum")> Public Property n_spectrum As UInteger
''' <summary>
''' the unique id of the root reference spectrum data for current cluster tree node
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("root"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="root")> Public Property root As UInteger
''' <summary>
''' the md5 hash key of the tree pathfor search a specific tree node quickly
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hash_index"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hash_index")> Public Property hash_index As String
''' <summary>
''' the tree path depth of current cluster node
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("depth"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="depth")> Public Property depth As UInteger
    <DatabaseField("consensus"), DataType(MySqlDbType.VarChar, "4096"), Column(Name:="consensus")> Public Property consensus As String
''' <summary>
''' the annotation of the current spectrum cluster data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("annotations"), DataType(MySqlDbType.Text), Column(Name:="annotations")> Public Property annotations As String
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `cluster` (`model_id`, `key`, `parent_id`, `n_childs`, `n_spectrum`, `root`, `hash_index`, `depth`, `consensus`, `annotations`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `cluster` (`id`, `model_id`, `key`, `parent_id`, `n_childs`, `n_spectrum`, `root`, `hash_index`, `depth`, `consensus`, `annotations`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `cluster` (`model_id`, `key`, `parent_id`, `n_childs`, `n_spectrum`, `root`, `hash_index`, `depth`, `consensus`, `annotations`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `cluster` (`id`, `model_id`, `key`, `parent_id`, `n_childs`, `n_spectrum`, `root`, `hash_index`, `depth`, `consensus`, `annotations`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `cluster` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `cluster` SET `id`='{0}', `model_id`='{1}', `key`='{2}', `parent_id`='{3}', `n_childs`='{4}', `n_spectrum`='{5}', `root`='{6}', `hash_index`='{7}', `depth`='{8}', `consensus`='{9}', `annotations`='{10}', `add_time`='{11}' WHERE `id` = '{12}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `cluster` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `cluster` (`id`, `model_id`, `key`, `parent_id`, `n_childs`, `n_spectrum`, `root`, `hash_index`, `depth`, `consensus`, `annotations`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, model_id, key, parent_id, n_childs, n_spectrum, root, hash_index, depth, consensus, annotations, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `cluster` (`id`, `model_id`, `key`, `parent_id`, `n_childs`, `n_spectrum`, `root`, `hash_index`, `depth`, `consensus`, `annotations`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, model_id, key, parent_id, n_childs, n_spectrum, root, hash_index, depth, consensus, annotations, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, model_id, key, parent_id, n_childs, n_spectrum, root, hash_index, depth, consensus, annotations, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{model_id}', '{key}', '{parent_id}', '{n_childs}', '{n_spectrum}', '{root}', '{hash_index}', '{depth}', '{consensus}', '{annotations}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{model_id}', '{key}', '{parent_id}', '{n_childs}', '{n_spectrum}', '{root}', '{hash_index}', '{depth}', '{consensus}', '{annotations}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `cluster` (`id`, `model_id`, `key`, `parent_id`, `n_childs`, `n_spectrum`, `root`, `hash_index`, `depth`, `consensus`, `annotations`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, model_id, key, parent_id, n_childs, n_spectrum, root, hash_index, depth, consensus, annotations, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `cluster` (`id`, `model_id`, `key`, `parent_id`, `n_childs`, `n_spectrum`, `root`, `hash_index`, `depth`, `consensus`, `annotations`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, model_id, key, parent_id, n_childs, n_spectrum, root, hash_index, depth, consensus, annotations, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, model_id, key, parent_id, n_childs, n_spectrum, root, hash_index, depth, consensus, annotations, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `cluster` SET `id`='{0}', `model_id`='{1}', `key`='{2}', `parent_id`='{3}', `n_childs`='{4}', `n_spectrum`='{5}', `root`='{6}', `hash_index`='{7}', `depth`='{8}', `consensus`='{9}', `annotations`='{10}', `add_time`='{11}' WHERE `id` = '{12}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, model_id, key, parent_id, n_childs, n_spectrum, root, hash_index, depth, consensus, annotations, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As cluster
                         Return DirectCast(MyClass.MemberwiseClone, cluster)
                     End Function
End Class


End Namespace
