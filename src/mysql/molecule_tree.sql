CREATE DATABASE  IF NOT EXISTS `molecule_tree` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `molecule_tree`;
-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: 192.168.0.231    Database: molecule_tree
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
-- Table structure for table `atoms`
--

DROP TABLE IF EXISTS `atoms`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `atoms` (
  `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'the unique reference id of current atom group, in integer id representative',
  `unique_id` varchar(64) NOT NULL COMMENT 'the unique reference id of current atom group, in character representative',
  `atom_group` varchar(45) NOT NULL COMMENT 'the atom group name, could be duplicated',
  `element` varchar(8) NOT NULL COMMENT 'the base element atom of current group data',
  `aromatic` int unsigned NOT NULL DEFAULT '0' COMMENT 'on an aromatic ring? 1 means true',
  `hydrogen` int unsigned NOT NULL DEFAULT '0' COMMENT 'The number of the hydrogen of current atom group it has',
  `charge` int NOT NULL DEFAULT '0' COMMENT 'the ion charge value of current atom group, could be a positive/negative integer value',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'create time of this atom group',
  `note` longtext COMMENT 'description about this atom group data',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `unique_id_UNIQUE` (`unique_id`) /*!80000 INVISIBLE */,
  KEY `find_reference` (`unique_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='atom group information data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `graph`
--

DROP TABLE IF EXISTS `graph`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='the molecule structure graph';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `model`
--

DROP TABLE IF EXISTS `model`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `model` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL DEFAULT 'unnamed',
  `cluster_cutoff` double NOT NULL DEFAULT '0.9' COMMENT 'cosine score cutoff for make cluster into one cluster node',
  `right` double NOT NULL DEFAULT '0.6' COMMENT 'cosine score cutoff for put the node to right',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='cluster tree model parameters';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `models`
--

DROP TABLE IF EXISTS `models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `models` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL DEFAULT 'unnamed',
  `cluster_cutoff` double NOT NULL DEFAULT '0.9' COMMENT 'cosine score cutoff for make cluster into one cluster node',
  `right` double NOT NULL DEFAULT '0.6' COMMENT 'cosine score cutoff for put the node to right',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='cluster tree model parameters';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `molecule_atoms`
--

DROP TABLE IF EXISTS `molecule_atoms`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `molecule_atoms` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `molecule` int unsigned NOT NULL,
  `index` int unsigned NOT NULL COMMENT 'duplictaed atom groups amybe contains in a molecule graph, needs used the index for make these duplictaed item different',
  `atom_id` int unsigned NOT NULL,
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `check_atom_reference` (`molecule`,`index`,`atom_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='relationship between a molecule and atom groups';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `molecules`
--

DROP TABLE IF EXISTS `molecules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `molecules` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `db_xref` varchar(255) NOT NULL COMMENT 'the unique reference id of current molecule from the external database',
  `name` varchar(2048) NOT NULL COMMENT 'common name of the metabolite',
  `formula` varchar(64) NOT NULL COMMENT 'formula string of the molecule',
  `exact_mass` double unsigned NOT NULL DEFAULT '0',
  `smiles` longtext NOT NULL COMMENT 'a unique representive canonical smiles structure data',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `db_xref_UNIQUE` (`db_xref`),
  KEY `find_by_xref` (`db_xref`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='cache pool of external molecule data set';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tree`
--

DROP TABLE IF EXISTS `tree`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tree` (
  `id` int unsigned NOT NULL AUTO_INCREMENT COMMENT 'id of the edge',
  `model_id` int unsigned NOT NULL,
  `parent_id` int unsigned NOT NULL COMMENT '[id] of the parent node of current node',
  `graph_id` int unsigned NOT NULL COMMENT 'the molecule structure graph reference id of current node',
  `cosine` double unsigned NOT NULL DEFAULT '0' COMMENT 'cosine similarity of current node when compares with the parent node',
  `jaccard` double unsigned NOT NULL DEFAULT '0' COMMENT 'jaccard similarity index of the current node when compares with the parent node',
  `t` double unsigned NOT NULL,
  `pvalue` double unsigned NOT NULL,
  `left` int unsigned NOT NULL COMMENT 'tree node id of left\n',
  `right` int unsigned NOT NULL COMMENT 'tree node id of right',
  `add_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'create time of current cluster node',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `graph_data_index` (`graph_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='the cluster tree network data';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-05  9:15:50
