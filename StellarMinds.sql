USE StellarMinds;
GO

-- =====================================================================
-- LIMPIEZA DE DATOS EXISTENTES
-- Orden inverso a las FK para evitar conflictos de integridad referencial
-- =====================================================================
DELETE FROM [Observaciones];
DELETE FROM [AuditoriasPrestamo];
DELETE FROM [Prestamos];
DELETE FROM [Equipos];
DELETE FROM [ObjetosCelestes];
DELETE FROM [Usuarios];

-- Reiniciar identidades para que los IDs arranquen desde 1
DBCC CHECKIDENT ('[Observaciones]',      RESEED, 0);
DBCC CHECKIDENT ('[AuditoriasPrestamo]', RESEED, 0);
DBCC CHECKIDENT ('[Prestamos]',          RESEED, 0);
DBCC CHECKIDENT ('[Equipos]',            RESEED, 0);
DBCC CHECKIDENT ('[ObjetosCelestes]',    RESEED, 0);
DBCC CHECKIDENT ('[Usuarios]',           RESEED, 0);
GO

-- =====================================================================
-- EQUIPOS
-- Tabla TPH (Table Per Hierarchy): Telescopio, Montura, Camara, Ocular
-- Columna Discriminator identifica el tipo concreto (max 13 chars)
-- TipoMontura: ECUATORIAL=0, ALTAZIMUTAL=1, HIBRIDA=2
-- =====================================================================
SET IDENTITY_INSERT [Equipos] ON;

INSERT INTO [Equipos]
    (Id, Marca, Modelo, Stock, Discriminator,
     Apertura, RelFocal, DistanciaFocal, Peso,
     Tipo, CargaSoportada, Computorizado,
     Sensor, Resolucion, Pixel,
     Diametro, Angulo)
VALUES
-- Telescopios
(1,  'Celestron',         'NexStar 8SE',     3, 'Telescopio', 203.20, 'f/10', 2032.00,  5.40, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(2,  'Meade',             'LX90-8',          2, 'Telescopio', 203.20, 'f/10', 2032.00,  6.50, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(3,  'Sky-Watcher',       'Explorer 200P',   4, 'Telescopio', 200.00, 'f/6',  1200.00,  7.20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(4,  'Orion',             'SkyView Pro 6',   3, 'Telescopio', 150.00, 'f/5',   750.00,  4.80, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(5,  'Takahashi',         'FSQ-85ED',        2, 'Telescopio',  85.00, 'f/5.3', 450.00,  3.10, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
-- Monturas
-- TipoMontura: ECUATORIAL=0, ALTAZIMUTAL=1, HIBRIDA=2
(6,  'Sky-Watcher',       'EQ6-R Pro',       3, 'Montura',    NULL,   NULL,   NULL,     NULL, 0,    20.00, 1,    NULL, NULL, NULL, NULL, NULL),
(7,  'Celestron',         'AVX',             2, 'Montura',    NULL,   NULL,   NULL,     NULL, 0,    18.00, 1,    NULL, NULL, NULL, NULL, NULL),
(8,  'Meade',             'LT-Alt-Az',       4, 'Montura',    NULL,   NULL,   NULL,     NULL, 1,    15.00, 0,    NULL, NULL, NULL, NULL, NULL),
(9,  'Explore Scientific','Twilight I',      3, 'Montura',    NULL,   NULL,   NULL,     NULL, 1,    12.00, 0,    NULL, NULL, NULL, NULL, NULL),
(10, 'Celestron',         'CGEM-II',         2, 'Montura',    NULL,   NULL,   NULL,     NULL, 0,    18.00, 1,    NULL, NULL, NULL, NULL, NULL),
-- Camaras
(11, 'ZWO',               'ASI294MC Pro',    3, 'Camara',     NULL,   NULL,   NULL,     NULL, NULL, NULL,  NULL, 'CMOS', '4144x2822', 4.63, NULL, NULL),
(12, 'QHY',               'QHY268C',         2, 'Camara',     NULL,   NULL,   NULL,     NULL, NULL, NULL,  NULL, 'CMOS', '6280x4210', 3.76, NULL, NULL),
(13, 'Atik',              '383L+',           3, 'Camara',     NULL,   NULL,   NULL,     NULL, NULL, NULL,  NULL, 'CCD',  '3362x2504', 5.40, NULL, NULL),
(14, 'ZWO',               'ASI1600MM Pro',   4, 'Camara',     NULL,   NULL,   NULL,     NULL, NULL, NULL,  NULL, 'CMOS', '4656x3520', 3.80, NULL, NULL),
(15, 'Canon',             'EOS Ra',          2, 'Camara',     NULL,   NULL,   NULL,     NULL, NULL, NULL,  NULL, 'CMOS', '6720x4480', 4.36, NULL, NULL),
-- Oculares
(16, 'Explore Scientific','68deg 24mm',      5, 'Ocular',     NULL,   NULL,   NULL,     NULL, NULL, NULL,  NULL, NULL,   NULL,        NULL, 24.00, 68.00),
(17, 'Televue',           'Nagler 13mm',     4, 'Ocular',     NULL,   NULL,   NULL,     NULL, NULL, NULL,  NULL, NULL,   NULL,        NULL, 13.00, 82.00),
(18, 'Baader',            'Hyperion 21mm',   6, 'Ocular',     NULL,   NULL,   NULL,     NULL, NULL, NULL,  NULL, NULL,   NULL,        NULL, 21.00, 68.00),
(19, 'Orion',             'Stratus 17.5mm',  5, 'Ocular',     NULL,   NULL,   NULL,     NULL, NULL, NULL,  NULL, NULL,   NULL,        NULL, 17.50, 68.00),
(20, 'Meade',             'UWA 14mm',        3, 'Ocular',     NULL,   NULL,   NULL,     NULL, NULL, NULL,  NULL, NULL,   NULL,        NULL, 14.00, 82.00);

SET IDENTITY_INSERT [Equipos] OFF;
GO

-- =====================================================================
-- OBJETOS CELESTES
-- TipoObjetoCeleste: PLANETA=0, ESTRELLA=1, GALAXIA=2, NEBULOSA=3
-- Magnitud: valor astronomico real (negativo = mas brillante)
-- =====================================================================
SET IDENTITY_INSERT [ObjetosCelestes] ON;

INSERT INTO [ObjetosCelestes] (Id, Nombre, Tipo, Magnitud)
VALUES
(1,  'Marte',                      0, -2.94),
(2,  'Jupiter',                    0, -2.20),
(3,  'Saturno',                    0,  0.46),
(4,  'Venus',                      0, -4.14),
(5,  'Sirio',                      1, -1.46),
(6,  'Betelgeuse',                 1,  0.42),
(7,  'Via Lactea',                 2,  3.10),
(8,  'Galaxia de Andromeda M31',   2,  3.44),
(9,  'Nebulosa de Orion M42',      3,  4.00),
(10, 'Nebulosa Cabeza de Caballo', 3,  5.60),
(11, 'Cumulo de Hercules M13',     1,  5.80),
(12, 'Galaxia del Remolino M51',   2,  8.60);

SET IDENTITY_INSERT [ObjetosCelestes] OFF;
GO

-- =====================================================================
-- USUARIOS
-- RolDeUsuario: ADMINISTRADOR=0, COORDINADOR=1, SOCIO=2
-- Contrasena_Pass: minimo 8 chars, 1 mayuscula, 1 minuscula,
--                  1 numero, 1 especial de: !@#$%^&*
-- =====================================================================
SET IDENTITY_INSERT [Usuarios] ON;

INSERT INTO [Usuarios]
    (Id, NombreCompleto_nombreCompleto,
     Direccion_Calle, Direccion_Numero, Direccion_Apartamento,
     Direccion_Esquina, Direccion_Departamento, Direccion_Pais,
     Telefono, Mail_Mail, NombreUsuario, Contrasena_Pass, Rol)
VALUES
-- ADMINISTRADOR (Rol=0)
(1,  'Juan Perez',
     'Av. Brasil',        1250, 0, 'Av. Italia',     'Montevideo', 'Uruguay',
     '094100001', 'admin@stellar.uy',      'admin',    'Admin1@stellar',   0),

-- COORDINADORES (Rol=1)
(2,  'Maria Gonzalez',
     'Av. 18 de Julio',   880,  2, 'Rio Branco',     'Montevideo', 'Uruguay',
     '094100002', 'mgonzalez@stellar.uy',  'coord1',   'Coord1@2026!',     1),
(3,  'Carlos Lopez',
     'Colonia',           1540, 0, 'Yi',             'Montevideo', 'Uruguay',
     '094100003', 'clopez@stellar.uy',     'coord2',   'Coord2@2026!',     1),
(4,  'Ana Martinez',
     'Rivera',            3200, 4, 'Gral. Flores',   'Montevideo', 'Uruguay',
     '094100004', 'amartinez@stellar.uy',  'coord3',   'Coord3@2026!',     1),

-- SOCIOS (Rol=2)
(5,  'Pedro Suarez',
     'Bulevar Artigas',   1100, 0, 'Av. Italia',     'Montevideo', 'Uruguay',
     '094100005', 'psuarez@stellar.uy',    'socio01',  'Socio01#2026',     2),
(6,  'Laura Fernandez',
     'Dr. P. de Pena',    1900, 3, 'Av. Sarmiento',  'Montevideo', 'Uruguay',
     '094100006', 'lfernandez@stellar.uy', 'socio02',  'Socio02#2026',     2),
(7,  'Diego Herrera',
     'Paysandu',           765, 0, 'Paraguay',       'Montevideo', 'Uruguay',
     '094100007', 'dherrera@stellar.uy',   'socio03',  'Socio03#2026',     2),
(8,  'Sofia Castro',
     'Ejido',             1860, 0, 'San Jose',       'Montevideo', 'Uruguay',
     '094100008', 'scastro@stellar.uy',    'socio04',  'Socio04#2026',     2),
(9,  'Martin Gomez',
     'Jackson',           1386, 1, 'Paysandu',       'Montevideo', 'Uruguay',
     '094100009', 'mgomez@stellar.uy',     'socio05',  'Socio05#2026',     2),
(10, 'Valentina Silva',
     'Andes',             1280, 0, 'Canelones',      'Montevideo', 'Uruguay',
     '094100010', 'vsilva@stellar.uy',     'socio06',  'Socio06#2026',     2),
(11, 'Nicolas Perez',
     'Cuareim',            944, 0, 'Paraguay',       'Montevideo', 'Uruguay',
     '094100011', 'nperez@stellar.uy',     'socio07',  'Socio07#2026',     2),
(12, 'Camila Torres',
     'Maldonado',         1100, 2, 'Yaguaron',       'Montevideo', 'Uruguay',
     '094100012', 'ctorres@stellar.uy',    'socio08',  'Socio08#2026',     2),
(13, 'Rodrigo Diaz',
     'Mercedes',           972, 0, 'Cerrito',        'Montevideo', 'Uruguay',
     '094100013', 'rdiaz@stellar.uy',      'socio09',  'Socio09#2026',     2),
(14, 'Florencia Reyes',
     'Soriano',           1314, 0, 'Convencion',     'Montevideo', 'Uruguay',
     '094100014', 'freyes@stellar.uy',     'socio10',  'Socio10#2026',     2);

SET IDENTITY_INSERT [Usuarios] OFF;
GO

-- =====================================================================
-- PRESTAMOS
-- EstadoPrestamo: PRESTAMO=0, DEVUELTO=1
--
-- Reglas de negocio validadas:
--   (a) FechaFin > FechaInicio
--   (b) Al menos un CamaraId o un OcularId (no ambos nulos)
--   (c) Si CamaraId != NULL: Montura.Tipo debe ser ECUATORIAL(0) o HIBRIDA(2)
--   (d) Montura.CargaSoportada >= Telescopio.Peso
--   (e) Stock de todos los equipos involucrados > 0
--
-- Referencia de monturas:
--   Id=6  Sky-Watcher EQ6-R Pro   ECUATORIAL(0) carga=20kg  stock=3
--   Id=7  Celestron AVX           ECUATORIAL(0) carga=18kg  stock=2
--   Id=8  Meade LT-Alt-Az         ALTAZIMUTAL(1) carga=15kg  stock=4
--   Id=9  Explore Scientific TwI  ALTAZIMUTAL(1) carga=12kg  stock=3
--   Id=10 Celestron CGEM-II       ECUATORIAL(0) carga=18kg  stock=2
--
-- Referencia de telescopios (peso):
--   Id=1 5.40kg | Id=2 6.50kg | Id=3 7.20kg | Id=4 4.80kg | Id=5 3.10kg
-- =====================================================================
SET IDENTITY_INSERT [Prestamos] ON;

INSERT INTO [Prestamos]
    (Id, FechaInicio, FechaFin, Estado, SocioId, TelescopioId, MonturaId, CamaraId, OcularId)
VALUES
-- Con camara (requiere montura ECUATORIAL o HIBRIDA) - DEVUELTOS (Estado=1)
(1,  '2025-03-01', '2025-03-08', 1, 5,  1, 6,  11,   NULL),  -- Tel1(5.4kg) Mont6(20kg ECUAT) Cam11
(2,  '2025-03-15', '2025-03-22', 1, 6,  2, 7,  12,   NULL),  -- Tel2(6.5kg) Mont7(18kg ECUAT) Cam12
(3,  '2025-04-01', '2025-04-08', 1, 7,  3, 6,  13,   NULL),  -- Tel3(7.2kg) Mont6(20kg ECUAT) Cam13
(4,  '2025-04-15', '2025-04-22', 1, 8,  4, 10, 14,   16),    -- Tel4(4.8kg) Mont10(18kg ECUAT) Cam14+Ocul16
(5,  '2025-05-01', '2025-05-08', 1, 9,  5, 7,  15,   17),    -- Tel5(3.1kg) Mont7(18kg ECUAT) Cam15+Ocul17
-- Solo con ocular (cualquier tipo de montura) - DEVUELTOS (Estado=1)
(6,  '2025-05-15', '2025-05-22', 1, 10, 1, 8,  NULL, 18),    -- Tel1(5.4kg) Mont8(15kg ALTAZ) Ocul18
(7,  '2025-06-01', '2025-06-08', 1, 11, 2, 9,  NULL, 19),    -- Tel2(6.5kg) Mont9(12kg ALTAZ) Ocul19
(8,  '2025-06-15', '2025-06-22', 1, 12, 3, 6,  NULL, 20),    -- Tel3(7.2kg) Mont6(20kg ECUAT) Ocul20
(9,  '2025-07-01', '2025-07-08', 1, 13, 4, 8,  NULL, 16),    -- Tel4(4.8kg) Mont8(15kg ALTAZ) Ocul16
(10, '2025-07-15', '2025-07-22', 1, 14, 5, 9,  NULL, 17),    -- Tel5(3.1kg) Mont9(12kg ALTAZ) Ocul17
-- Activos (Estado=0 = PRESTAMO)
(11, '2026-05-20', '2026-06-20', 0, 5,  1, 7,  11,   18),    -- Tel1(5.4kg) Mont7(18kg ECUAT) Cam11+Ocul18
(12, '2026-05-22', '2026-06-22', 0, 6,  2, 10, 12,   19),    -- Tel2(6.5kg) Mont10(18kg ECUAT) Cam12+Ocul19
(13, '2026-05-25', '2026-06-25', 0, 7,  4, 8,  NULL, 20),    -- Tel4(4.8kg) Mont8(15kg ALTAZ) Ocul20
(14, '2026-05-28', '2026-06-28', 0, 8,  5, 9,  NULL, 16),    -- Tel5(3.1kg) Mont9(12kg ALTAZ) Ocul16
(15, '2026-05-30', '2026-06-30', 0, 9,  3, 6,  13,   NULL);  -- Tel3(7.2kg) Mont6(20kg ECUAT) Cam13

SET IDENTITY_INSERT [Prestamos] OFF;
GO

-- =====================================================================
-- AUDITORIAS DE PRESTAMO
-- AccionAuditoria: ALTA_PRESTAMO=0, DEVOLUCION=1
-- CoordinadorId: debe ser un usuario con Rol=COORDINADOR (IDs 2, 3, 4)
-- =====================================================================
SET IDENTITY_INSERT [AuditoriasPrestamo] ON;

INSERT INTO [AuditoriasPrestamo] (Id, Accion, Fecha, CoordinadorId, PrestamoId)
VALUES
-- Alta de cada prestamo (Accion=0)
(1,  0, '2025-03-01', 2, 1),
(2,  0, '2025-03-15', 3, 2),
(3,  0, '2025-04-01', 4, 3),
(4,  0, '2025-04-15', 2, 4),
(5,  0, '2025-05-01', 3, 5),
(6,  0, '2025-05-15', 4, 6),
(7,  0, '2025-06-01', 2, 7),
(8,  0, '2025-06-15', 3, 8),
(9,  0, '2025-07-01', 4, 9),
(10, 0, '2025-07-15', 2, 10),
(11, 0, '2026-05-20', 3, 11),
(12, 0, '2026-05-22', 4, 12),
(13, 0, '2026-05-25', 2, 13),
(14, 0, '2026-05-28', 3, 14),
(15, 0, '2026-05-30', 4, 15),
-- Devolucion de prestamos devueltos (Accion=1) - solo para Estado=DEVUELTO (IDs 1-10)
(16, 1, '2025-03-08', 2, 1),
(17, 1, '2025-03-22', 3, 2),
(18, 1, '2025-04-08', 4, 3),
(19, 1, '2025-04-22', 2, 4),
(20, 1, '2025-05-08', 3, 5),
(21, 1, '2025-05-22', 4, 6),
(22, 1, '2025-06-08', 2, 7),
(23, 1, '2025-06-22', 3, 8),
(24, 1, '2025-07-08', 4, 9),
(25, 1, '2025-07-22', 2, 10);

SET IDENTITY_INSERT [AuditoriasPrestamo] OFF;
GO

-- =====================================================================
-- OBSERVACIONES
-- IndicadorAdecuacion: IDEAL=0, ADECUADO=1, NO_RECOMENDABLE=2
-- TipoObservacion:     VISUAL=0, ASTROFOTOGRAFICA=1
-- =====================================================================
SET IDENTITY_INSERT [Observaciones] ON;

INSERT INTO [Observaciones]
    (Id, Fecha, PrestamoId, ObjetoId, SocioId, Indicador, Detalle, TipoObservacion)
VALUES
-- Observaciones de prestamos devueltos
(1,  '2025-03-02', 1,  1,  5,  0, 'Marte visto con gran detalle, condiciones atmosfericas excelentes. Detalle polar distinguible.', 1),
(2,  '2025-03-03', 1,  2,  5,  0, 'Jupiter con las cuatro lunas galileanas perfectamente visibles. Banda ecuatorial bien marcada.', 1),
(3,  '2025-03-16', 2,  3,  6,  0, 'Anillos de Saturno con gran definicion. Division de Cassini claramente separada.', 1),
(4,  '2025-04-02', 3,  9,  7,  1, 'Nebulosa de Orion observada con detalle. Se distingue el Trapecio central.', 1),
(5,  '2025-04-03', 3,  12, 7,  0, 'Galaxia del Remolino M51 con estructura espiral levemente visible.', 1),
(6,  '2025-04-16', 4,  4,  8,  0, 'Venus en fase gibbosa. Color blanquecino brillante bien definido.', 0),
(7,  '2025-04-17', 4,  5,  8,  1, 'Sirio en zona baja del horizonte, centelleos notorios por atmosfera.', 0),
(8,  '2025-05-02', 5,  8,  9,  0, 'Galaxia de Andromeda M31 bien definida. Nucleo compacto distinguible.', 1),
(9,  '2025-05-16', 6,  10, 10, 2, 'Nebulosa Cabeza de Caballo apenas distinguible por alta contaminacion luminica.', 0),
(10, '2025-06-02', 7,  11, 11, 1, 'Cumulo globular M13 magnifico. Miles de estrellas resueltas con ocular de 13mm.', 0),
(11, '2025-06-16', 8,  6,  12, 0, 'Betelgeuse con tonos rojizos inconfundibles. Noche tranquila, sin viento.', 0),
(12, '2025-07-02', 9,  1,  13, 0, 'Marte en oposicion. Casquete polar sur visible. Condiciones ideales.', 0),
(13, '2025-07-16', 10, 2,  14, 1, 'Jupiter con banda ecuatorial doble marcada. Gran mancha roja visible.', 0),
-- Observaciones de prestamos activos
(14, '2026-05-21', 11, 3,  5,  0, 'Saturno fotografiado con camara. Anillos perfectamente separados. Titan distinguible.', 1),
(15, '2026-05-23', 12, 7,  6,  0, 'Via Lactea en su esplendor. Noche de luna nueva ideal para gran campo.', 1),
(16, '2026-05-26', 13, 9,  7,  1, 'Nebulosa de Orion observada visualmente. Buenas condiciones de seeing.', 0),
(17, '2026-05-29', 14, 5,  8,  0, 'Sirio observado en noche clara. Estrella compania (Sirio B) no resuelta.', 0),
(18, '2026-05-31', 15, 4,  9,  0, 'Venus en maximo brillo. Fase en cuarto creciente bien definida.', 1);

SET IDENTITY_INSERT [Observaciones] OFF;
GO
