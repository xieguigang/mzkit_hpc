#Region "Microsoft.VisualBasic::a25832724de47d54c79b15f6b46fbd47, Rscript\Library\mzkit_hpc\src\MoleculeTree\models\tree.vb"

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

    '   Total Lines: 255
    '    Code Lines: 95 (37.25%)
    ' Comment Lines: 138 (54.12%)
    '    - Xml Docs: 95.65%
    ' 
    '   Blank Lines: 22 (8.63%)
    '     File Size: 12.58 KB


    ' Class tree
    ' 
    '     Properties: add_time, cosine, graph_id, id, jaccard
    '                 left, model_id, parent_id, pvalue, right
    '                 t
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

REM  Dump @5/28/2025 02:55:30 PM


Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports MySqlScript = Oracle.LinuxCompatibility.MySQL.Scripting.Extensions

Namespace treeModel

''' <summary>
''' ```SQL
''' 
''' --
''' 
''' DROP TABLE IF EXISTS `tree`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `tree` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'id of the edge',
'''   `model_id` int unsigned NOT NULL,
'''   `parent_id` int unsigned NOT NULL COMMENT '[id] of the parent node of current node',
'''   `graph_id` int unsigned NOT NULL COMMENT 'the molecule structure graph reference id of current node',
'''   `cosine` double unsigned NOT NULL DEFAULT '0' COMMENT 'cosine similarity of current node when compares with the parent node',
'''   `jaccard` double unsigned NOT NULL DEFAULT '0' COMMENT 'jaccard similarity index of the current node when compares with the parent node',
'''   `t` double NOT NULL COMMENT 'test value, maybe negative',
'''   `pvalue` double unsigned NOT NULL,
'''   `left` int unsigned NOT NULL COMMENT 'tree node id of left\n',
'''   `right` int unsigned NOT NULL COMMENT 'tree node id of right',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'create time of current cluster node',
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   KEY `graph_data_index` (`graph_id`),
'''   KEY `find_node` (`model_id`,`graph_id`)
''' ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='the cluster tree network data';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' -- Dumping events for database 'molecule_tree'
''' --
''' 
''' --
''' -- Dumping routines for database 'molecule_tree'
''' --
''' /*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;
''' 
''' /*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
''' /*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
''' /*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
''' /*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
''' /*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
''' /*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
''' /*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
''' 
''' -- Dump completed on 2024-12-21 23:33:12
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("tree", Database:="molecule_tree", SchemaSQL:="
CREATE TABLE `tree` (
  `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'id of the edge',
  `model_id` int unsigned NOT NULL,
  `parent_id` int unsigned NOT NULL COMMENT '[id] of the parent node of current node',
  `graph_id` int unsigned NOT NULL COMMENT 'the molecule structure graph reference id of current node',
  `cosine` double unsigned NOT NULL DEFAULT '0' COMMENT 'cosine similarity of current node when compares with the parent node',
  `jaccard` double unsigned NOT NULL DEFAULT '0' COMMENT 'jaccard similarity index of the current node when compares with the parent node',
  `t` double NOT NULL COMMENT 'test value, maybe negative',
  `pvalue` double unsigned NOT NULL,
  `left` int unsigned NOT NULL COMMENT 'tree node id of left\n',
  `right` int unsigned NOT NULL COMMENT 'tree node id of right',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'create time of current cluster node',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `graph_data_index` (`graph_id`),
  KEY `find_node` (`model_id`,`graph_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='the cluster tree network data';")>
Public Class tree: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
''' <summary>
''' id of the edge
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
    <DatabaseField("model_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="model_id")> Public Property model_id As UInteger
''' <summary>
''' [id] of the parent node of current node
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("parent_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="parent_id")> Public Property parent_id As UInteger
''' <summary>
''' the molecule structure graph reference id of current node
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("graph_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="graph_id")> Public Property graph_id As UInteger
''' <summary>
''' cosine similarity of current node when compares with the parent node
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("cosine"), NotNull, DataType(MySqlDbType.Double), Column(Name:="cosine")> Public Property cosine As Double
''' <summary>
''' jaccard similarity index of the current node when compares with the parent node
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("jaccard"), NotNull, DataType(MySqlDbType.Double), Column(Name:="jaccard")> Public Property jaccard As Double
''' <summary>
''' test value, maybe negative
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("t"), NotNull, DataType(MySqlDbType.Double), Column(Name:="t")> Public Property t As Double
    <DatabaseField("pvalue"), NotNull, DataType(MySqlDbType.Double), Column(Name:="pvalue")> Public Property pvalue As Double
''' <summary>
''' tree node id of left\n
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("left"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="left")> Public Property left As UInteger
''' <summary>
''' tree node id of right
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("right"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="right")> Public Property right As UInteger
''' <summary>
''' create time of current cluster node
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `tree` (`model_id`, `parent_id`, `graph_id`, `cosine`, `jaccard`, `t`, `pvalue`, `left`, `right`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `tree` (`id`, `model_id`, `parent_id`, `graph_id`, `cosine`, `jaccard`, `t`, `pvalue`, `left`, `right`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `tree` (`model_id`, `parent_id`, `graph_id`, `cosine`, `jaccard`, `t`, `pvalue`, `left`, `right`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `tree` (`id`, `model_id`, `parent_id`, `graph_id`, `cosine`, `jaccard`, `t`, `pvalue`, `left`, `right`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `tree` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `tree` SET `id`='{0}', `model_id`='{1}', `parent_id`='{2}', `graph_id`='{3}', `cosine`='{4}', `jaccard`='{5}', `t`='{6}', `pvalue`='{7}', `left`='{8}', `right`='{9}', `add_time`='{10}' WHERE `id` = '{11}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `tree` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `tree` (`id`, `model_id`, `parent_id`, `graph_id`, `cosine`, `jaccard`, `t`, `pvalue`, `left`, `right`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, model_id, parent_id, graph_id, cosine, jaccard, t, pvalue, left, right, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `tree` (`id`, `model_id`, `parent_id`, `graph_id`, `cosine`, `jaccard`, `t`, `pvalue`, `left`, `right`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, model_id, parent_id, graph_id, cosine, jaccard, t, pvalue, left, right, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, model_id, parent_id, graph_id, cosine, jaccard, t, pvalue, left, right, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{model_id}', '{parent_id}', '{graph_id}', '{cosine}', '{jaccard}', '{t}', '{pvalue}', '{left}', '{right}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{model_id}', '{parent_id}', '{graph_id}', '{cosine}', '{jaccard}', '{t}', '{pvalue}', '{left}', '{right}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `tree` (`id`, `model_id`, `parent_id`, `graph_id`, `cosine`, `jaccard`, `t`, `pvalue`, `left`, `right`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, model_id, parent_id, graph_id, cosine, jaccard, t, pvalue, left, right, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `tree` (`id`, `model_id`, `parent_id`, `graph_id`, `cosine`, `jaccard`, `t`, `pvalue`, `left`, `right`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, model_id, parent_id, graph_id, cosine, jaccard, t, pvalue, left, right, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, model_id, parent_id, graph_id, cosine, jaccard, t, pvalue, left, right, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `tree` SET `id`='{0}', `model_id`='{1}', `parent_id`='{2}', `graph_id`='{3}', `cosine`='{4}', `jaccard`='{5}', `t`='{6}', `pvalue`='{7}', `left`='{8}', `right`='{9}', `add_time`='{10}' WHERE `id` = '{11}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, model_id, parent_id, graph_id, cosine, jaccard, t, pvalue, left, right, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As tree
                         Return DirectCast(MyClass.MemberwiseClone, tree)
                     End Function
End Class


End Namespace
