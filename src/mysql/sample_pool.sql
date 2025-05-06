CREATE DATABASE  IF NOT EXISTS `sample_pool` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `sample_pool`;
-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: 192.168.0.231    Database: sample_pool
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `cluster`
--

DROP TABLE IF EXISTS `cluster`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cluster` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `model_id` int unsigned NOT NULL COMMENT 'the graph model reference id',
  `key` varchar(64) NOT NULL COMMENT 'the key name of current cluster tree node',
  `parent_id` int unsigned NOT NULL DEFAULT '0' COMMENT 'default value -1 means current tree node is root node',
  `n_childs` int unsigned NOT NULL DEFAULT '0' COMMENT 'the number of childs that contains in current cluster tree node',
  `n_spectrum` int unsigned NOT NULL DEFAULT '0' COMMENT 'the cluster size: number of the spectrum data that inside this cluster data',
  `root` int unsigned NOT NULL DEFAULT '0' COMMENT 'the unique id of the root reference spectrum data for current cluster tree node',
  `hash_index` char(32) NOT NULL COMMENT 'the md5 hash key of the tree pathfor search a specific tree node quickly',
  `depth` int unsigned NOT NULL DEFAULT '1' COMMENT 'the tree path depth of current cluster node',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `consensus` varchar(4096) DEFAULT '*',
  `annotations` mediumtext COMMENT 'the annotation of the current spectrum cluster data',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `spectrum_index` (`root`) /*!80000 INVISIBLE */,
  KEY `parent_node` (`parent_id`) /*!80000 INVISIBLE */,
  KEY `hash_key` (`hash_index`) /*!80000 INVISIBLE */,
  KEY `model_index` (`model_id`),
  KEY `cluster_index` (`hash_index`,`model_id`),
  KEY `spectrum_size_index` (`n_spectrum`) /*!80000 INVISIBLE */,
  KEY `child_size_index` (`n_childs`)
) ENGINE=InnoDB AUTO_INCREMENT=3089322 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cluster_data`
--

DROP TABLE IF EXISTS `cluster_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cluster_data` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `cluster_id` int unsigned NOT NULL COMMENT 'the cluster tree node id, this id could be duplicated, this data field indicates that which cluster is belongs to of current spectram data',
  `model_id` int unsigned NOT NULL,
  `metadata_id` int unsigned NOT NULL COMMENT 'the metabolite annotation id of the current spectrum data',
  `spectral_id` int unsigned NOT NULL COMMENT 'the spectrum data reference id',
  `score` float NOT NULL DEFAULT '0' COMMENT 'the score value of current spectrum that align with the root spectrum of current cluster',
  `n_hits` int unsigned NOT NULL DEFAULT '0' COMMENT 'number of the ms2 ion fragment hits in the spectrum aignment operation',
  `forward` float NOT NULL DEFAULT '0',
  `reverse` float NOT NULL DEFAULT '0',
  `jaccard` float NOT NULL DEFAULT '0',
  `entropy` float NOT NULL DEFAULT '0',
  `p_value` double NOT NULL DEFAULT '1' COMMENT 't-test p-value of [forward,reverse,jaccard,entropy]',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `consensus` varchar(4096) DEFAULT NULL COMMENT 'consensus ions m/z value, network byte order data stream in base64 string',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `model_index` (`model_id`),
  KEY `meta_id` (`metadata_id`),
  KEY `query_score_item` (`cluster_id`,`spectral_id`),
  KEY `find_spectra` (`spectral_id`)
) ENGINE=InnoDB AUTO_INCREMENT=10583380 DEFAULT CHARSET=utf8mb3 COMMENT='the cluster data link of the spectrum members in current cluster tree node';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cluster_graph`
--

DROP TABLE IF EXISTS `cluster_graph`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cluster_graph` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `model_id` int NOT NULL,
  `cluster_id` int unsigned NOT NULL,
  `link_to` int unsigned NOT NULL,
  `similarity` double NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `model_index` (`model_id`),
  KEY `cluster_index1` (`cluster_id`) /*!80000 INVISIBLE */,
  KEY `cluster_index2` (`link_to`) /*!80000 INVISIBLE */,
  KEY `score_index` (`similarity`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='the networking result based on the cluster table consensus stat result';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cluster_tree`
--

DROP TABLE IF EXISTS `cluster_tree`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cluster_tree` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `cluster_node` int NOT NULL COMMENT 'the current cluster tree node id',
  `child_node` int NOT NULL COMMENT 'the one of the cluster tree child of current cluster tree node',
  `depth` int NOT NULL,
  `model_id` int NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `cluster_1` (`cluster_node`) /*!80000 INVISIBLE */,
  KEY `cluster_2` (`child_node`),
  KEY `model_index` (`model_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3089315 DEFAULT CHARSET=utf8mb3 COMMENT='the cluster tree data structrue';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `consensus_model`
--

DROP TABLE IF EXISTS `consensus_model`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `consensus_model` (
  `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'the parameter id',
  `model_id` int unsigned NOT NULL,
  `consensus_cutoff` double unsigned NOT NULL DEFAULT '0.3',
  `umap_dimension` int unsigned NOT NULL DEFAULT '10',
  `umap_neighbors` int unsigned NOT NULL DEFAULT '15',
  `umap_others` varchar(1024) NOT NULL DEFAULT '{}',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `model_info_idx` (`model_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='model and parameters for create consensus analysis result';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `consensus_spectrum`
--

DROP TABLE IF EXISTS `consensus_spectrum`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `consensus_spectrum` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `cluster_id` int unsigned NOT NULL,
  `parameter_id` int unsigned NOT NULL COMMENT 'parameter id reference of cutoff range and umap parameters',
  `spectrum` varchar(4096) NOT NULL COMMENT 'consensus peak and intensity value, base64strng encoded of double number in network byteorder',
  `peak_ranking` varchar(4096) NOT NULL COMMENT 'consensus peak and ranking value, base64 string encoded double numbers in network byteorder',
  `umap` varchar(2048) NOT NULL COMMENT 'base64string for umap result of multiple dimension number encoded in network byte order',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `cluster_info_idx` (`cluster_id`),
  KEY `parameter_info_idx` (`parameter_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `graph_model`
--

DROP TABLE IF EXISTS `graph_model`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `graph_model` (
  `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'the molecule networking model reference id',
  `name` varchar(255) NOT NULL COMMENT 'the model name',
  `parameters` varchar(4096) NOT NULL COMMENT 'json string for the model parameters',
  `description` text,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `flag` int NOT NULL DEFAULT '0' COMMENT 'delete flag',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `metadata`
--

DROP TABLE IF EXISTS `metadata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `metadata` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `hashcode` varchar(32) NOT NULL,
  `mz` double NOT NULL DEFAULT '-1',
  `rt` double NOT NULL DEFAULT '0',
  `intensity` double NOT NULL DEFAULT '0',
  `filename` varchar(255) NOT NULL,
  `cluster_id` int unsigned NOT NULL,
  `rawfile` int unsigned NOT NULL,
  `spectral_id` int unsigned NOT NULL,
  `model_id` int unsigned NOT NULL,
  `project_id` int NOT NULL DEFAULT '-1',
  `project` varchar(255) NOT NULL DEFAULT 'NA',
  `biosample` varchar(64) NOT NULL,
  `organism` varchar(128) NOT NULL,
  `biodeep_id` varchar(32) NOT NULL COMMENT 'biodeep_000000001  or unknown conserved',
  `name` varchar(4096) NOT NULL,
  `formula` varchar(64) NOT NULL,
  `adducts` varchar(32) NOT NULL,
  `instrument` varchar(255) NOT NULL DEFAULT 'Thermo Scientific Q Exactive',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `idmetadata_UNIQUE` (`id`),
  KEY `project_index` (`project_id`) /*!80000 INVISIBLE */,
  KEY `project_index2` (`project`) /*!80000 INVISIBLE */,
  KEY `model_index` (`model_id`),
  KEY `metadata_query` (`hashcode`,`cluster_id`,`model_id`) /*!80000 INVISIBLE */,
  KEY `check_duplicated` (`hashcode`,`filename`,`cluster_id`,`model_id`,`project`,`biodeep_id`),
  KEY `biosample_stats` (`organism`,`biosample`) /*!80000 INVISIBLE */,
  KEY `cluster_reference_index` (`cluster_id`),
  KEY `metadata_query_index` (`id`,`model_id`,`cluster_id`),
  KEY `mass_index` (`mz`),
  KEY `time_index` (`rt`)
) ENGINE=InnoDB AUTO_INCREMENT=10583381 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `project`
--

DROP TABLE IF EXISTS `project`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `project` (
  `id` int NOT NULL AUTO_INCREMENT,
  `project_id` varchar(128) NOT NULL,
  `project_name` varchar(255) NOT NULL,
  `add_time` datetime NOT NULL,
  `sample_groups` int NOT NULL DEFAULT '0',
  `sample_files` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`,`project_id`),
  UNIQUE KEY `idproject_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rawfiles`
--

DROP TABLE IF EXISTS `rawfiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rawfiles` (
  `id` int NOT NULL AUTO_INCREMENT,
  `filename` varchar(256) NOT NULL,
  `size_bytes` double NOT NULL DEFAULT '0',
  `add_time` datetime NOT NULL,
  `project_id` int NOT NULL,
  `sample_group` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sample_groups`
--

DROP TABLE IF EXISTS `sample_groups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sample_groups` (
  `id` int NOT NULL AUTO_INCREMENT,
  `group_name` varchar(64) NOT NULL,
  `project_id` int NOT NULL,
  `sample_files` int NOT NULL DEFAULT '0',
  `polarity` int NOT NULL DEFAULT '1',
  `organism` varchar(255) NOT NULL,
  `bio_samples` varchar(255) NOT NULL,
  `repo_path` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `spectrum_pool`
--

DROP TABLE IF EXISTS `spectrum_pool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `spectrum_pool` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `npeaks` int NOT NULL DEFAULT '0' COMMENT 'number of ion peaks in current spectrum data',
  `entropy` double NOT NULL DEFAULT '0',
  `hashcode` char(32) NOT NULL COMMENT 'md5 hash code of the spectrum data',
  `model_id` int NOT NULL,
  `mz` mediumtext NOT NULL COMMENT 'mz double array data in network byte order and base64 encoding, andalso the byte data is compress in gzip',
  `into` mediumtext NOT NULL COMMENT 'intensity double array data in network byte order and base64 encoding, andalso the byte data is compress in gzip',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `model_index` (`model_id`),
  KEY `peak_hash_index` (`hashcode`),
  KEY `find_spectrum1` (`id`,`model_id`),
  KEY `find_spectrum2` (`hashcode`,`model_id`)
) ENGINE=InnoDB AUTO_INCREMENT=10583386 DEFAULT CHARSET=utf8mb3 COMMENT='the spectrum data pool';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'sample_pool'
--

--
-- Dumping routines for database 'sample_pool'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-06 10:38:42
