REM  Oracle.LinuxCompatibility.MySQL.CodeSolution.VisualBasic.CodeGenerator
REM  MYSQL Schema Mapper
REM      for Microsoft VisualBasic.NET 1.0.0.0

REM  Dump @12/5/2024 10:27:34 AM


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
''' DROP TABLE IF EXISTS `graph`;
''' /*!40101 SET @saved_cs_client     = @@character_set_client */;
''' /*!50503 SET character_set_client = utf8mb4 */;
''' CREATE TABLE `graph` (
'''   `id` int unsigned NOT NULL AUTO_INCREMENT,
'''   `molecule_id` int unsigned NOT NULL COMMENT 'the unique reference id of the molecule from external database records',
'''   `hashcode` varchar(32) NOT NULL COMMENT 'fast hascode check of the graph matrix under current atoms layout',
'''   `graph` json NOT NULL COMMENT 'network graph model',
'''   `smiles` longtext NOT NULL COMMENT 'smiles structure data of current molecule, may contains variant data',
'''   `matrix` longtext NOT NULL COMMENT 'matrix representive of the graph json, base64 encoded double[] matrix data',
'''   `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'create time of current molecule graph model',
'''   PRIMARY KEY (`id`),
'''   UNIQUE KEY `id_UNIQUE` (`id`),
'''   KEY `check_unique_graph` (`molecule_id`,`hashcode`)
''' ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='the molecule structure graph';
''' /*!40101 SET character_set_client = @saved_cs_client */;
''' 
''' --
''' ```
''' </summary>
''' <remarks></remarks>
<Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes.TableName("graph", Database:="molecule_tree", SchemaSQL:="
CREATE TABLE `graph` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule_id` int unsigned NOT NULL COMMENT 'the unique reference id of the molecule from external database records',
  `hashcode` varchar(32) NOT NULL COMMENT 'fast hascode check of the graph matrix under current atoms layout',
  `graph` json NOT NULL COMMENT 'network graph model',
  `smiles` longtext NOT NULL COMMENT 'smiles structure data of current molecule, may contains variant data',
  `matrix` longtext NOT NULL COMMENT 'matrix representive of the graph json, base64 encoded double[] matrix data',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'create time of current molecule graph model',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `check_unique_graph` (`molecule_id`,`hashcode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='the molecule structure graph';")>
Public Class graph: Inherits Oracle.LinuxCompatibility.MySQL.MySQLTable
#Region "Public Property Mapping To Database Fields"
    <DatabaseField("id"), PrimaryKey, AutoIncrement, NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="id"), XmlAttribute> Public Property id As UInteger
''' <summary>
''' the unique reference id of the molecule from external database records
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("molecule_id"), NotNull, DataType(MySqlDbType.UInt32, "11"), Column(Name:="molecule_id")> Public Property molecule_id As UInteger
''' <summary>
''' fast hascode check of the graph matrix under current atoms layout
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("hashcode"), NotNull, DataType(MySqlDbType.VarChar, "32"), Column(Name:="hashcode")> Public Property hashcode As String
''' <summary>
''' network graph model
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("graph"), NotNull, DataType(MySqlDbType.Text), Column(Name:="graph")> Public Property graph As String
''' <summary>
''' smiles structure data of current molecule, may contains variant data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("smiles"), NotNull, DataType(MySqlDbType.Text), Column(Name:="smiles")> Public Property smiles As String
''' <summary>
''' matrix representive of the graph json, base64 encoded double[] matrix data
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("matrix"), NotNull, DataType(MySqlDbType.Text), Column(Name:="matrix")> Public Property matrix As String
''' <summary>
''' create time of current molecule graph model
''' </summary>
''' <value></value>
''' <returns></returns>
''' <remarks></remarks>
    <DatabaseField("add_time"), NotNull, DataType(MySqlDbType.DateTime), Column(Name:="add_time")> Public Property add_time As Date
#End Region
#Region "Public SQL Interface"
#Region "Interface SQL"
    Friend Shared ReadOnly INSERT_SQL$ = 
        <SQL>INSERT INTO `graph` (`molecule_id`, `hashcode`, `graph`, `smiles`, `matrix`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly INSERT_AI_SQL$ = 
        <SQL>INSERT INTO `graph` (`id`, `molecule_id`, `hashcode`, `graph`, `smiles`, `matrix`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly REPLACE_SQL$ = 
        <SQL>REPLACE INTO `graph` (`molecule_id`, `hashcode`, `graph`, `smiles`, `matrix`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');</SQL>

    Friend Shared ReadOnly REPLACE_AI_SQL$ = 
        <SQL>REPLACE INTO `graph` (`id`, `molecule_id`, `hashcode`, `graph`, `smiles`, `matrix`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');</SQL>

    Friend Shared ReadOnly DELETE_SQL$ =
        <SQL>DELETE FROM `graph` WHERE `id` = '{0}';</SQL>

    Friend Shared ReadOnly UPDATE_SQL$ = 
        <SQL>UPDATE `graph` SET `id`='{0}', `molecule_id`='{1}', `hashcode`='{2}', `graph`='{3}', `smiles`='{4}', `matrix`='{5}', `add_time`='{6}' WHERE `id` = '{7}';</SQL>

#End Region

''' <summary>
''' ```SQL
''' DELETE FROM `graph` WHERE `id` = '{0}';
''' ```
''' </summary>
    Public Overrides Function GetDeleteSQL() As String
        Return String.Format(DELETE_SQL, id)
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `graph` (`id`, `molecule_id`, `hashcode`, `graph`, `smiles`, `matrix`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL() As String
        Return String.Format(INSERT_SQL, molecule_id, hashcode, graph, smiles, matrix, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' INSERT INTO `graph` (`id`, `molecule_id`, `hashcode`, `graph`, `smiles`, `matrix`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetInsertSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(INSERT_AI_SQL, id, molecule_id, hashcode, graph, smiles, matrix, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(INSERT_SQL, molecule_id, hashcode, graph, smiles, matrix, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' <see cref="GetInsertSQL"/>
''' </summary>
    Public Overrides Function GetDumpInsertValue(AI As Boolean) As String
        If AI Then
            Return $"('{id}', '{molecule_id}', '{hashcode}', '{graph}', '{smiles}', '{matrix}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        Else
            Return $"('{molecule_id}', '{hashcode}', '{graph}', '{smiles}', '{matrix}', '{add_time.ToString("yyyy-MM-dd hh:mm:ss")}')"
        End If
    End Function


''' <summary>
''' ```SQL
''' REPLACE INTO `graph` (`id`, `molecule_id`, `hashcode`, `graph`, `smiles`, `matrix`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL() As String
        Return String.Format(REPLACE_SQL, molecule_id, hashcode, graph, smiles, matrix, MySqlScript.ToMySqlDateTimeString(add_time))
    End Function

''' <summary>
''' ```SQL
''' REPLACE INTO `graph` (`id`, `molecule_id`, `hashcode`, `graph`, `smiles`, `matrix`, `add_time`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');
''' ```
''' </summary>
    Public Overrides Function GetReplaceSQL(AI As Boolean) As String
        If AI Then
        Return String.Format(REPLACE_AI_SQL, id, molecule_id, hashcode, graph, smiles, matrix, MySqlScript.ToMySqlDateTimeString(add_time))
        Else
        Return String.Format(REPLACE_SQL, molecule_id, hashcode, graph, smiles, matrix, MySqlScript.ToMySqlDateTimeString(add_time))
        End If
    End Function

''' <summary>
''' ```SQL
''' UPDATE `graph` SET `id`='{0}', `molecule_id`='{1}', `hashcode`='{2}', `graph`='{3}', `smiles`='{4}', `matrix`='{5}', `add_time`='{6}' WHERE `id` = '{7}';
''' ```
''' </summary>
    Public Overrides Function GetUpdateSQL() As String
        Return String.Format(UPDATE_SQL, id, molecule_id, hashcode, graph, smiles, matrix, MySqlScript.ToMySqlDateTimeString(add_time), id)
    End Function
#End Region

''' <summary>
                     ''' Memberwise clone of current table Object.
                     ''' </summary>
                     Public Function Clone() As graph
                         Return DirectCast(MyClass.MemberwiseClone, graph)
                     End Function
End Class


End Namespace
