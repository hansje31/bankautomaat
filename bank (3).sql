-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Gegenereerd op: 20 jan 2021 om 17:47
-- Serverversie: 10.4.14-MariaDB
-- PHP-versie: 7.4.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bank`
--

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `bankdetails`
--

CREATE TABLE `bankdetails` (
  `bank_nummer_id` int(11) NOT NULL,
  `bank_rekeningnummer` int(255) NOT NULL,
  `deleted_state` tinyint(1) NOT NULL DEFAULT 0,
  `saldo` decimal(8,2) NOT NULL,
  `pin` varchar(255) NOT NULL,
  `user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `bankdetails`
--

INSERT INTO `bankdetails` (`bank_nummer_id`, `bank_rekeningnummer`, `deleted_state`, `saldo`, `pin`, `user_id`) VALUES
(1, 326235237, 0, '999289.99', '$2a$11$TmFgNAz29s70gn5PONFtheV1mkefDRCr1Gpyqa2oKJBRIBkjtX7XK', 1),
(2, 742442121, 0, '0.00', '$2a$11$B9b4czgzzTD3Pu45BXARQ.XKbwZ4z6qtcX2rijNmJa3NtIq4hh3Ha', 2);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `transacties`
--

CREATE TABLE `transacties` (
  `transactie_id` int(11) NOT NULL,
  `datum` timestamp NOT NULL DEFAULT current_timestamp(),
  `type` varchar(255) NOT NULL,
  `bedrag` decimal(6,2) NOT NULL,
  `bank_nummer_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `transacties`
--

INSERT INTO `transacties` (`transactie_id`, `datum`, `type`, `bedrag`, `bank_nummer_id`) VALUES
(1, '2021-01-14 10:16:10', 'Deposit', '100.00', 1),
(2, '2021-01-14 10:16:19', 'Deposit', '9999.99', 1),
(3, '2021-01-14 10:17:13', 'Withdraw', '-500.00', 1),
(4, '2021-01-19 08:27:39', 'Deposit', '100.00', 1),
(5, '2021-01-19 08:29:01', 'Withdraw', '-10.00', 1),
(6, '2021-01-19 08:29:04', 'Withdraw', '-10.00', 1),
(7, '2021-01-19 08:29:25', 'Withdraw', '-300.00', 1),
(8, '2021-01-19 08:29:39', 'Deposit', '10.00', 1);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `user`
--

CREATE TABLE `user` (
  `user_id` int(11) NOT NULL,
  `voornaam` varchar(255) NOT NULL,
  `achternaam` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `geslacht` varchar(255) NOT NULL,
  `straat` varchar(255) NOT NULL,
  `postcode` varchar(255) NOT NULL,
  `stad` varchar(255) NOT NULL,
  `telefoonnummer` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `user`
--

INSERT INTO `user` (`user_id`, `voornaam`, `achternaam`, `email`, `geslacht`, `straat`, `postcode`, `stad`, `telefoonnummer`) VALUES
(1, 'test', 'test', 'test@gmail.com', 'male', 'mattenbies 31', '3824WB', 'amersfoort', '6133846'),
(2, 'Hans', 'de Jong', 'hansdejong21@gmail.com', 'male', 'shsdjhs', '2832ewh', 'amersfoort', '838387383');

--
-- Indexen voor geëxporteerde tabellen
--

--
-- Indexen voor tabel `bankdetails`
--
ALTER TABLE `bankdetails`
  ADD PRIMARY KEY (`bank_nummer_id`);

--
-- Indexen voor tabel `transacties`
--
ALTER TABLE `transacties`
  ADD PRIMARY KEY (`transactie_id`);

--
-- Indexen voor tabel `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT voor geëxporteerde tabellen
--

--
-- AUTO_INCREMENT voor een tabel `bankdetails`
--
ALTER TABLE `bankdetails`
  MODIFY `bank_nummer_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT voor een tabel `transacties`
--
ALTER TABLE `transacties`
  MODIFY `transactie_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT voor een tabel `user`
--
ALTER TABLE `user`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
