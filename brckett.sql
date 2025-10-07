CREATE DATABASE brckett
CHARSET utf8
COLLATE utf8_hungarian_ci;

-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Sze 19. 09:26
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `brckett`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `groups`
--

CREATE TABLE `groups` (
  `group_id` int(11) NOT NULL,
  `group_name` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `groupscheduleconn`
--

CREATE TABLE `groupscheduleconn` (
  `group_id` int(11) NOT NULL,
  `schedule_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `groupuserconn`
--

CREATE TABLE `groupuserconn` (
  `user_Id` int(11) NOT NULL,
  `group_Id` int(11) NOT NULL,
  `permission` varchar(128) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `schedules`
--

CREATE TABLE `schedules` (
  `template_id` int(11) DEFAULT NULL,
  `schedule_info` varchar(255) DEFAULT NULL,
  `schedule_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `schedulesusersconn`
--

CREATE TABLE `schedulesusersconn` (
  `user_id` int(11) NOT NULL,
  `schedule_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `templates`
--

CREATE TABLE `templates` (
  `template_id` int(11) NOT NULL,
  `template_info` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `users`
--

CREATE TABLE `users` (
  `user_id` int(11) NOT NULL,
  `username` varchar(128) NOT NULL,
  `email` varchar(128) NOT NULL,
  `display_name` varchar(128) DEFAULT NULL,
  `PASSWORD` varchar(128) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `usersettings`
--

CREATE TABLE `usersettings` (
  `user_id` int(11) NOT NULL,
  `settings` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `groups`
--
ALTER TABLE `groups`
  ADD PRIMARY KEY (`group_id`);

--
-- A tábla indexei `groupscheduleconn`
--
ALTER TABLE `groupscheduleconn`
  ADD PRIMARY KEY (`group_id`,`schedule_id`),
  ADD KEY `schedule_id` (`schedule_id`);

--
-- A tábla indexei `groupuserconn`
--
ALTER TABLE `groupuserconn`
  ADD PRIMARY KEY (`user_Id`,`group_Id`),
  ADD KEY `group_Id` (`group_Id`);

--
-- A tábla indexei `schedules`
--
ALTER TABLE `schedules`
  ADD PRIMARY KEY (`schedule_id`),
  ADD KEY `template_id` (`template_id`);

--
-- A tábla indexei `schedulesusersconn`
--
ALTER TABLE `schedulesusersconn`
  ADD PRIMARY KEY (`user_id`,`schedule_id`),
  ADD KEY `schedule_id` (`schedule_id`);

--
-- A tábla indexei `templates`
--
ALTER TABLE `templates`
  ADD PRIMARY KEY (`template_id`);

--
-- A tábla indexei `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_id`);

--
-- A tábla indexei `usersettings`
--
ALTER TABLE `usersettings`
  ADD PRIMARY KEY (`user_id`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `groups`
--
ALTER TABLE `groups`
  MODIFY `group_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `schedules`
--
ALTER TABLE `schedules`
  MODIFY `schedule_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `templates`
--
ALTER TABLE `templates`
  MODIFY `template_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `users`
--
ALTER TABLE `users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `groupscheduleconn`
--
ALTER TABLE `groupscheduleconn`
  ADD CONSTRAINT `groupscheduleconn_ibfk_1` FOREIGN KEY (`schedule_id`) REFERENCES `schedules` (`schedule_id`),
  ADD CONSTRAINT `groupscheduleconn_ibfk_2` FOREIGN KEY (`group_id`) REFERENCES `groups` (`group_id`);

--
-- Megkötések a táblához `groupuserconn`
--
ALTER TABLE `groupuserconn`
  ADD CONSTRAINT `groupuserconn_ibfk_1` FOREIGN KEY (`user_Id`) REFERENCES `users` (`user_id`),
  ADD CONSTRAINT `groupuserconn_ibfk_2` FOREIGN KEY (`group_Id`) REFERENCES `groups` (`group_id`);

--
-- Megkötések a táblához `schedules`
--
ALTER TABLE `schedules`
  ADD CONSTRAINT `schedules_ibfk_1` FOREIGN KEY (`template_id`) REFERENCES `templates` (`template_id`),
  ADD CONSTRAINT `schedules_ibfk_2` FOREIGN KEY (`template_id`) REFERENCES `templates` (`template_id`);

--
-- Megkötések a táblához `schedulesusersconn`
--
ALTER TABLE `schedulesusersconn`
  ADD CONSTRAINT `schedulesusersconn_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`),
  ADD CONSTRAINT `schedulesusersconn_ibfk_2` FOREIGN KEY (`schedule_id`) REFERENCES `schedules` (`schedule_id`);

--
-- Megkötések a táblához `usersettings`
--
ALTER TABLE `usersettings`
  ADD CONSTRAINT `usersettings_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
