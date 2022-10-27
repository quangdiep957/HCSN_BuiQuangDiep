﻿--
-- Script was generated by Devart dbForge Studio for MySQL, Version 8.0.40.0
-- Product home page: http://www.devart.com/dbforge/mysql/studio
-- Script date 27/01/2021 11:41:32 AM
-- Server version: 8.0.19
-- Client version: 4.1
--

-- 
-- Disable foreign keys
-- 
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;

-- 
-- Set SQL mode
-- 
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 
-- Set character set the client will use to send SQL statements to the server
--
SET NAMES 'utf8';

--
-- Set default database
--
USE qlts_fresher_core;

--
-- Drop table `department`
--
DROP TABLE IF EXISTS department;

--
-- Drop table `fixed_asset`
--
DROP TABLE IF EXISTS fixed_asset;

--
-- Drop table `fixed_asset_category`
--
DROP TABLE IF EXISTS fixed_asset_category;

--
-- Set default database
--
USE qlts_fresher_core;

--
-- Create table `fixed_asset_category`
--
CREATE TABLE fixed_asset_category (
  fixed_asset_category_id char(36) NOT NULL COMMENT 'Id loại tài sản',
  fixed_asset_category_code varchar(50) DEFAULT NULL COMMENT 'Mã loại tài sản',
  fixed_asset_category_name varchar(255) DEFAULT NULL COMMENT 'Tên loại tài sản',
  organization_id char(36) DEFAULT NULL COMMENT 'Id của đơn vị',
  depreciation_rate float DEFAULT NULL COMMENT 'Tỷ lệ hao mòn (%)',
  life_time int DEFAULT NULL COMMENT 'Số năm sử dụng ',
  description varchar(500) DEFAULT NULL COMMENT 'Ghi chú',
  created_by varchar(50) DEFAULT NULL COMMENT 'Người tạo',
  created_date timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Ngày tạo',
  modified_by varchar(50) DEFAULT NULL COMMENT 'Người sửa',
  modified_date timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Ngày sửa',
  PRIMARY KEY (fixed_asset_category_id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci,
COMMENT = 'Loại tài sản';

--
-- Create table `fixed_asset`
--
CREATE TABLE fixed_asset (
  fixed_asset_id char(36) NOT NULL COMMENT 'Id tài sản',
  fixed_asset_code varchar(20) DEFAULT NULL COMMENT 'Mã tài sản',
  fixed_asset_name varchar(255) DEFAULT NULL COMMENT 'Tên tài sản',
  organization_id char(36) DEFAULT NULL COMMENT 'Id của đơn vị',
  organization_code varchar(50) DEFAULT NULL COMMENT 'Mã đơn vị',
  organization_name varchar(255) DEFAULT NULL COMMENT 'Tên của đơn vị',
  department_id char(36) DEFAULT NULL COMMENT 'Id phòng ban',
  department_code varchar(50) DEFAULT NULL COMMENT 'Mã phòng ban',
  department_name varchar(255) DEFAULT NULL,
  fixed_asset_category_id char(36) DEFAULT NULL COMMENT 'Id loại tài sản',
  fixed_asset_category_code varchar(50) DEFAULT NULL COMMENT 'Mã loại tài sản',
  fixed_asset_category_name varchar(255) DEFAULT NULL COMMENT 'Tên loại tài sản',
  purchase_date timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Ngày mua',
  cost decimal(19, 4) DEFAULT NULL COMMENT 'nguyên giá',
  quantity int DEFAULT 0 COMMENT 'Số lượng',
  depreciation_rate float DEFAULT NULL COMMENT 'Tỷ lệ hao mòn (%)',
  tracked_year int DEFAULT NULL COMMENT 'Năm bắt đầu theo dõi tài sản trên phần mềm',
  life_time int DEFAULT NULL COMMENT 'Số năm sử dụng ',
  production_year int DEFAULT NULL COMMENT 'Năm sử dụng',
  active bit(1) DEFAULT b'0' COMMENT 'Sử dụng',
  created_by varchar(50) DEFAULT NULL COMMENT 'Người tạo',
  created_date timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Ngày tạo',
  modified_by varchar(50) DEFAULT NULL COMMENT 'Người sửa',
  modified_date timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Ngày sửa',
  PRIMARY KEY (fixed_asset_id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `department`
--
CREATE TABLE department (
  department_id char(36) NOT NULL,
  department_code varchar(50) DEFAULT NULL COMMENT 'Mã của phòng ban',
  department_name varchar(255) DEFAULT NULL COMMENT 'Tên phòng ban',
  description varchar(500) DEFAULT NULL COMMENT 'Ghi chú',
  is_parent bit(1) DEFAULT NULL COMMENT 'Có phải là cha không',
  parent_id char(36) DEFAULT NULL COMMENT 'Id phòng ban cha',
  organization_id char(36) DEFAULT NULL COMMENT 'Id của đơn vị',
  created_by varchar(50) DEFAULT NULL COMMENT 'Người tạo',
  created_date timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Ngày tạo',
  modified_by varchar(50) DEFAULT NULL COMMENT 'Người sửa',
  modified_date timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Ngày sửa',
  PRIMARY KEY (department_id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci,
COMMENT = 'Danh mục phòng ban';

-- 
-- Dumping data for table fixed_asset_category
--
-- Table qlts_fresher_core.fixed_asset_category does not contain any data (it is empty)

-- 
-- Dumping data for table fixed_asset
--
-- Table qlts_fresher_core.fixed_asset does not contain any data (it is empty)

-- 
-- Dumping data for table department
--
-- Table qlts_fresher_core.department does not contain any data (it is empty)

-- 
-- Restore previous SQL mode
-- 
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;

-- 
-- Enable foreign keys
-- 
/*!40014 SET FOREIGN_KEY_CHECKS = @OLD_FOREIGN_KEY_CHECKS */;