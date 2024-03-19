-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 04-09-2023 a las 20:32:55
-- Versión del servidor: 10.4.25-MariaDB
-- Versión de PHP: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `id` int(11) NOT NULL,
  `idInquilino` int(11) NOT NULL,
  `idInmueble` int(11) NOT NULL,
  `montoMensual` double NOT NULL,
  `fechaInicio` date NOT NULL,
  `fechaFin` date NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`id`, `idInquilino`, `idInmueble`, `montoMensual`, `fechaInicio`, `fechaFin`, `estado`) VALUES
(10, 13, 51, 85000, '2023-09-26', '2023-12-26', 1),
(11, 13, 1, 95000, '2023-06-29', '2023-08-29', 0),
(12, 12, 1, 189999, '2023-08-25', '2023-08-26', 0),
(24, 9, 64, 120000, '2023-08-28', '2023-08-29', 1),
(26, 13, 51, 220000, '2023-12-27', '2024-08-27', 1),
(28, 13, 51, 250000, '2024-08-28', '2025-05-28', 1),
(30, 9, 52, 150000, '2023-09-01', '2023-09-01', 0),
(31, 9, 52, 250000, '2025-02-01', '2027-02-01', 0),
(32, 12, 58, 180000, '2023-09-02', '2023-09-02', 0),
(33, 9, 72, 1850000, '2023-09-02', '2023-09-30', 1),
(34, 9, 58, 120000, '2023-09-03', '2023-09-10', 1),
(35, 9, 58, 250000, '2023-09-11', '2023-09-25', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `id` int(11) NOT NULL,
  `idPropietario` int(11) NOT NULL,
  `direccion` varchar(50) NOT NULL,
  `uso` enum('Comercial','Personal') NOT NULL,
  `tipo` enum('Local','Deposito','Casa','Departamento') NOT NULL,
  `cantAmbientes` int(11) NOT NULL,
  `latitud` double NOT NULL,
  `longitud` double NOT NULL,
  `precio` double NOT NULL,
  `estado` tinyint(1) NOT NULL,
  `disponible` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`id`, `idPropietario`, `direccion`, `uso`, `tipo`, `cantAmbientes`, `latitud`, `longitud`, `precio`, `estado`, `disponible`) VALUES
(1, 10, 'Calle San Martín 123', 'Personal', 'Departamento', 2, -33.2736103999, -66.3061100666, 105000, 1, 1),
(50, 10, 'San Martin 987', 'Comercial', 'Local', 3, -322736103652, -633061162548, 250000, 1, 1),
(51, 11, 'Av. del Sol 220', 'Comercial', 'Local', 3, -323459662332, -650102929894, 180000, 0, 0),
(52, 10, 'Calle Inventada 123', 'Comercial', 'Local', 3, -3544564545644, -65456456544, 150000, 1, 1),
(58, 35, 'Av. Illia 123', 'Comercial', 'Deposito', 3, -35.123456, -43.365412, 120000, 1, 1),
(64, 11, 'qweqwe', 'Comercial', 'Local', 34, 33, 43, 343434, 0, 1),
(65, 80, 'Ruta 5 123', 'Comercial', 'Deposito', 3, -32.123123123, -45.123123123, 123400, 0, 0),
(66, 85, 'Ruta 8 123', 'Comercial', 'Deposito', 4, -65.123456789, -45.789456123, 270000, 1, 1),
(67, 91, 'Calle Simple 123', 'Personal', 'Casa', 4, -65.456789789, -42.123456123, 135000, 0, 1),
(72, 10, 'Los Olivos 123', 'Personal', 'Departamento', 3, -48.789456, -63.456789, 185000, 1, 0),
(74, 94, 'ewrewrw', 'Comercial', 'Deposito', 9, 9, 9, 9, 0, 1),
(75, 99, 'sdsdadsadsadsa', 'Personal', 'Deposito', 2, -45.456456, -65.456456, 280000, 1, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id` int(11) NOT NULL,
  `dni` varchar(12) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `telefono` varchar(50) NOT NULL,
  `correo` varchar(50) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`id`, `dni`, `apellido`, `nombre`, `telefono`, `correo`, `estado`) VALUES
(9, '10.255.443', 'Gonzalez', 'Maria Cristina', '+54 266-5552156', 'maricrisgonz@gmail.com', 1),
(12, '35.855.658', 'Martinez', 'Juan', '+54 266-4690227', 'juanmaMartinez@gmail.com', 1),
(13, '25.365.325', 'De Luca', 'Roxana Maria', '+54 11-123456', 'roxi@mail.com', 1),
(30, '33.369.777', 'Gomez', 'Roberto', '+54 11-2365478', 'roberto@mail.com', 1),
(43, '21.321.789', 'Pelufo', 'Mariano', '+54 123-9874512', 'pelufom@mail.com', 1),
(49, '33.123.321', 'Vieyra', 'Moises', '+54 266-4123789', 'moises@mail.com', 0),
(68, '11.321.753', 'dfsfds', 'fdsfsdfds', '+54 123-456456', 'adsdsadsa@asdsad.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `id` int(11) NOT NULL,
  `nroPago` int(11) NOT NULL,
  `idContrato` int(11) NOT NULL,
  `fechaPago` datetime NOT NULL,
  `importe` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`id`, `nroPago`, `idContrato`, `fechaPago`, `importe`) VALUES
(19, 1, 10, '2023-08-29 00:00:00', 85000),
(20, 2, 10, '2023-09-29 00:00:00', 85000),
(21, 3, 10, '2023-10-29 00:00:00', 85000),
(22, 1, 11, '2023-09-22 00:00:00', 95000),
(24, 2, 11, '2023-10-22 00:00:00', 95000),
(26, 4, 10, '2023-11-26 00:00:00', 85000),
(36, 5, 10, '2023-09-26 00:00:00', 85000),
(37, 1, 28, '2024-08-28 00:00:00', 250000),
(38, 6, 10, '2023-09-26 00:00:00', 85000),
(39, 1, 32, '2023-09-02 00:00:00', 180000),
(41, 2, 32, '2023-09-03 00:00:00', 180000),
(42, 1, 33, '2023-09-03 00:00:00', 1850000),
(43, 1, 35, '2023-09-03 00:00:00', 250000);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id` int(11) NOT NULL,
  `dni` varchar(12) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `telefono` varchar(50) NOT NULL,
  `correo` varchar(50) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id`, `dni`, `apellido`, `nombre`, `telefono`, `correo`, `estado`) VALUES
(9, '35.656.321', 'Rodriguez', 'Lujan', '+54 011-123456', 'lujan2@mail.com', 1),
(10, '15.355.987', 'Chavez', 'Marta', '+54 354-456783', 'marta@mail.com', 1),
(11, '25.333.665', 'Baez', 'Juan Manuel', '+54 266-4698456', 'juanma@mail.com', 0),
(35, '35.321.456', 'Javier', 'Romero', '+54 266-4659789', 'javiromero@mail.com', 1),
(46, '11.111.111', 'Lucresia', 'Gomez', '+11 111-1111111', 'lucgomez@mail.com', 1),
(80, '35.222.333', 'Silva', 'Marco', '+55 555-555555', 'marco@mail.com', 0),
(85, '25.365.981', 'Luciano Agustin', 'Lopez', '+54 266-4690397', 'lulopez@mail.com', 1),
(91, '45.123.654', 'Alvarez', 'Julian', '+54 11-7894561', 'julianalvarez@mail.com', 0),
(94, '22.354.179', 'Dominguez', 'Monica', '+54 111-9874561', 'mdominguez@mail.com', 1),
(95, '43.657.324', 'sadsa', 'dasd', '+54 123-4896141', 'sadasdas@asdsa.com', 0),
(96, '11.222.671', 'AAAAAAAAAAAA', 'BBBBBBBBBBBB', '+54 142-9854587', 'sadasdas@asdsad.com', 0),
(97, '25.365.325', 'sadsads', 'adsadsd', '32434243242', 'roxi@mail.com', 1),
(98, '99.999.888', 'wqeqweqwe', 'ytuytuytuyt', '+99 999-999999', '999@mail.com', 1),
(99, '88.999.888', 'RRRRR', 'BBBBB', '+88 888-888888', '88888@mail.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `dni` varchar(50) NOT NULL,
  `telefono` varchar(50) NOT NULL,
  `correo` varchar(60) NOT NULL,
  `clave` varchar(255) NOT NULL,
  `avatar` varchar(255) NOT NULL,
  `estado` tinyint(1) NOT NULL,
  `rol` enum('Administrador','Empleado') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id`, `nombre`, `apellido`, `dni`, `telefono`, `correo`, `clave`, `avatar`, `estado`, `rol`) VALUES
(18, 'Marco', 'Silva', '35.111.333', '+54 266-690347', 'marcosilva@mail.com', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', '/Uploads\\avatar_18.jpg', 1, 'Administrador'),
(19, 'Pepe', 'Silvia', '22.333.213', '+54 266-4569874', 'pepe@mail.com', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', '/Uploads\\avatar_19.jpg', 0, 'Administrador'),
(23, 'Maria Cristina', 'Rodriguez', '22.654.984', '+54 222-789654', 'maria@mail.com', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', '/Uploads\\avatar_23_2b584a3a-802a-4d12-a1f5-bca5fd6ca26e.jpg', 1, 'Administrador'),
(24, 'Juan Manuel', 'Castro', '39.456.999', '+54 123-9999999', 'juan@mail.com', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', '/Uploads\\avatar_24_9ca7a5b0-cf15-4944-bdb3-ddfdc247d6e7.jpg', 1, 'Empleado'),
(34, 'Luciano', 'Gallardo', '35.321.761', '+54 266-4561236', 'lucho@mail.com', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', '/Uploads\\avatar_34.jpg', 1, 'Administrador'),
(35, 'sdasad', 'sadsad', '11.349.743', '+54 123-456456', 'sadsd@mail.com', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', '', 0, 'Empleado'),
(36, 'qweqwe', 'ewqewqe', '12.321.654', '+54 123-455465', 'maria@mail.comwqewe', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', '', 0, 'Administrador');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idInquilino_fk` (`idInquilino`),
  ADD KEY `idInmueble_fk` (`idInmueble`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idPropietario_fk` (`idPropietario`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idContrato_fk` (`idContrato`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=36;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=76;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=69;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=44;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=100;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=37;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `idInmueble_fk` FOREIGN KEY (`idInmueble`) REFERENCES `inmuebles` (`id`),
  ADD CONSTRAINT `idInquilino_fk` FOREIGN KEY (`idInquilino`) REFERENCES `inquilinos` (`id`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `idPropietario_fk` FOREIGN KEY (`idPropietario`) REFERENCES `propietarios` (`id`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `idContrato_fk` FOREIGN KEY (`idContrato`) REFERENCES `contratos` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
