-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: 2017-09-02 18:02:11
-- 服务器版本： 10.1.19-MariaDB
-- PHP Version: 7.0.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `nomatter`
--

-- --------------------------------------------------------

--
-- 表的结构 `matter_folder`
--

CREATE TABLE `matter_folder` (
  `folder_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `folder_name` tinytext CHARACTER SET utf8 COLLATE utf8_general_mysql500_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `session_info`
--

CREATE TABLE `session_info` (
  `session_id` int(10) UNSIGNED NOT NULL,
  `session_pass` char(32) NOT NULL,
  `session_info` text NOT NULL,
  `session_start_time` datetime NOT NULL,
  `session_last_time` datetime NOT NULL,
  `session_end_time` datetime NOT NULL,
  `session_status` int(11) NOT NULL DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- 表的结构 `user_info`
--

CREATE TABLE `user_info` (
  `user_id` bigint(20) UNSIGNED NOT NULL,
  `user_name` char(64) NOT NULL,
  `user_email` char(64) NOT NULL,
  `user_avatar` text NOT NULL,
  `user_nickname` char(64) NOT NULL,
  `user_password` char(32) NOT NULL,
  `user_phone` char(18) NOT NULL DEFAULT '1',
  `reg_ip` char(16) NOT NULL DEFAULT '',
  `login_ip` char(16) NOT NULL DEFAULT ''
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- 表的结构 `user_mailbox`
--

CREATE TABLE `user_mailbox` (
  `mailbox_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `email_address` tinytext CHARACTER SET utf8 COLLATE utf8_general_mysql500_ci NOT NULL,
  `email_password` tinytext CHARACTER SET utf8 COLLATE utf8_general_mysql500_ci NOT NULL,
  `pop3_address` tinytext CHARACTER SET utf8 COLLATE utf8_general_mysql500_ci NOT NULL,
  `pop3_port` int(11) NOT NULL,
  `use_ssl` tinyint(1) NOT NULL,
  `end_uid` text CHARACTER SET utf8 COLLATE utf8_general_mysql500_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `user_matters`
--

CREATE TABLE `user_matters` (
  `matter_id` int(10) UNSIGNED NOT NULL,
  `matter_type` char(32) NOT NULL,
  `user_id` int(11) NOT NULL,
  `matter_name` char(64) NOT NULL,
  `matter_desc` mediumtext NOT NULL,
  `matter_start_time` datetime NOT NULL,
  `matter_next_effect_time` datetime NOT NULL,
  `matter_end_time` datetime NOT NULL,
  `matter_addion_info` mediumtext NOT NULL,
  `folder_id` int(11) NOT NULL DEFAULT '0',
  `matter_status` int(11) NOT NULL DEFAULT '0',
  `is_noticed` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- 表的结构 `user_message`
--

CREATE TABLE `user_message` (
  `message_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `message_title` tinytext COLLATE utf8_general_mysql500_ci NOT NULL,
  `message_body` text COLLATE utf8_general_mysql500_ci NOT NULL,
  `create_time` datetime NOT NULL,
  `is_sended_email` tinyint(1) NOT NULL,
  `is_sended_sms` tinyint(1) NOT NULL,
  `is_sended_client` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `matter_folder`
--
ALTER TABLE `matter_folder`
  ADD PRIMARY KEY (`folder_id`),
  ADD KEY `user_id` (`user_id`);

--
-- Indexes for table `session_info`
--
ALTER TABLE `session_info`
  ADD PRIMARY KEY (`session_id`);

--
-- Indexes for table `user_info`
--
ALTER TABLE `user_info`
  ADD PRIMARY KEY (`user_id`),
  ADD UNIQUE KEY `user_name_2` (`user_name`),
  ADD KEY `user_name` (`user_name`),
  ADD KEY `user_email` (`user_email`);

--
-- Indexes for table `user_mailbox`
--
ALTER TABLE `user_mailbox`
  ADD PRIMARY KEY (`mailbox_id`),
  ADD KEY `user_id` (`user_id`);

--
-- Indexes for table `user_matters`
--
ALTER TABLE `user_matters`
  ADD PRIMARY KEY (`matter_id`),
  ADD KEY `matter_type` (`matter_type`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `folder_id` (`folder_id`),
  ADD KEY `matter_status` (`matter_status`);

--
-- Indexes for table `user_message`
--
ALTER TABLE `user_message`
  ADD PRIMARY KEY (`message_id`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `is_sended_email` (`is_sended_email`),
  ADD KEY `is_sended_sms` (`is_sended_sms`),
  ADD KEY `is_sended_client` (`is_sended_client`);

--
-- 在导出的表使用AUTO_INCREMENT
--

--
-- 使用表AUTO_INCREMENT `matter_folder`
--
ALTER TABLE `matter_folder`
  MODIFY `folder_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
--
-- 使用表AUTO_INCREMENT `session_info`
--
ALTER TABLE `session_info`
  MODIFY `session_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=678;
--
-- 使用表AUTO_INCREMENT `user_info`
--
ALTER TABLE `user_info`
  MODIFY `user_id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
--
-- 使用表AUTO_INCREMENT `user_mailbox`
--
ALTER TABLE `user_mailbox`
  MODIFY `mailbox_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- 使用表AUTO_INCREMENT `user_matters`
--
ALTER TABLE `user_matters`
  MODIFY `matter_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=75;
--
-- 使用表AUTO_INCREMENT `user_message`
--
ALTER TABLE `user_message`
  MODIFY `message_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
