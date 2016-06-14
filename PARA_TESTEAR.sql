--PARA TESTEAR--

SELECT * FROM GDD_15.DIRECCIONES -- 95 DIRECCIONES INSERTADAS--
SELECT * FROM GDD_15.USUARIOS --95 USU MIGRADOS + EL ADM CREADO --
SELECT * FROM GDD_15.CLIENTES --28 CLIENTES MIGRADOS--
SELECT * FROM GDD_15.EMPRESAS-- 67 EMPRESAS MIGRADAS--
SELECT * FROM #tmp_usuarios -- 95 USUARIOS MIGRADOS--
SELECT * FROM GDD_15.VISIBILIDADES -- 5 VISIBILIDADES INSERTADAS--
SELECT * FROM GDD_15.ROLES_USUARIOS --3 ROLES INSERTADOS--
SELECT * FROM GDD_15.FUNCIONALIDADES_ROLES --16 FUN_ROL INSERTADAS--
SELECT * FROM GDD_15.ROLES --3 ROLES INSERTADOS--
SELECT * FROM GDD_15.TIPOS
SELECT * FROM GDD_15.ESTADOS
-- NO HAY NADA TODAVIA-- LAS ESTOY MIGRANDO--
SELECT * FROM GDD_15.CALIFICACIONES 
SELECT * FROM GDD_15.OFERTAS
SELECT * FROM GDD_15.COMPRAS
SELECT * FROM GDD_15.RUBROS
SELECT * FROM GDD_15.PUBLICACIONES
SELECT * FROM GDD_15.FACTURAS
SELECT * FROM GDD_15.FACTURAS_ITEMS
GO

SELECT
(SELECT COUNT(*) CUENTA FROM GDD_15.CLIENTES CL JOIN GDD_15.COMPRAS CO ON (CL.N_ID_USUARIO = CO.N_ID_CLIENTE) WHERE CL.N_ID_USUARIO = 1) +
(SELECT COUNT(*) CUENTA FROM GDD_15.CLIENTES CL JOIN GDD_15.OFERTAS O ON (CL.N_ID_USUARIO = O.N_ID_CLIENTE) WHERE CL.N_ID_USUARIO = 1)

SELECT TOP 10 [C�digo Publicaci�n], Tipo, Descripci�n, [Monto ($)], Cantidad, Env�o, Ganador, [Fecha Operaci�n] FROM
(SELECT TOP 13 [C�digo Publicaci�n], Tipo, Descripci�n, [Monto ($)], Cantidad, Env�o, Ganador, [Fecha Operaci�n] FROM
(SELECT CO.N_ID_PUBLICACION 'C�digo Publicaci�n', 'Compra Inmediata' Tipo, P.D_DESCRED Descripci�n, N_CANTIDAD*N_PRECIO 'Monto ($)', N_CANTIDAD Cantidad, C_ENVIO Env�o, 'No aplica' Ganador, F_ALTA 'Fecha Operaci�n' FROM GDD_15.CLIENTES CL JOIN GDD_15.COMPRAS CO ON (CL.N_ID_USUARIO = CO.N_ID_CLIENTE) JOIN GDD_15.PUBLICACIONES P ON (CO.N_ID_PUBLICACION = P.N_ID_PUBLICACION) WHERE CL.N_ID_USUARIO = 1
UNION ALL SELECT O.N_ID_PUBLICACION 'C�digo Publicaci�n', 'Subasta' Tipo, P.D_DESCRED Descripci�n, N_MONTO 'Monto ($)', '1' Cantidad, C_ENVIO Env�o, C_GANADOR Ganador, F_ALTA 'Fecha Operaci�n' FROM GDD_15.CLIENTES CL JOIN GDD_15.OFERTAS O ON (CL.N_ID_USUARIO = O.N_ID_CLIENTE) JOIN GDD_15.PUBLICACIONES P ON (O.N_ID_PUBLICACION = P.N_ID_PUBLICACION) WHERE CL.N_ID_USUARIO = 1
) SQ ORDER BY [Fecha Operaci�n] DESC) SQ2 ORDER BY [Fecha Operaci�n]

SELECT F.N_ID_FACTURA 'C�digo Factura', N_ID_ITEM 'N� Item', 'Detalle', N_MONTO 'Monto ($)',SUM(N_MONTO) 'Total ($)', F.F_ALTA 'Fecha Alta' FROM GDD_15.PUBLICACIONES P JOIN GDD_15.FACTURAS F ON (P.N_ID_PUBLICACION = F.N_ID_PUBLICACION) JOIN GDD_15.FACTURAS_ITEMS FI ON (F.N_ID_FACTURA = FI.N_ID_FACTURA) WHERE N_ID_USUARIO = 97 GROUP BY F.N_ID_FACTURA, N_ID_ITEM, N_MONTO, F.F_ALTA

SELECT COUNT([C�digo Publicaci�n]) FROM
(SELECT CO.N_ID_PUBLICACION 'C�digo Publicaci�n', N_ID_COMPRA 'C�digo Compra u Oferta', 'Compra Inmediata' Tipo, P.D_DESCRED Descripci�n, N_CANTIDAD*N_PRECIO 'Monto ($)', N_CANTIDAD Cantidad, C_ENVIO Env�o, F_ALTA 'Fecha Operaci�n' FROM GDD_15.CLIENTES CL JOIN GDD_15.COMPRAS CO ON (CL.N_ID_USUARIO = CO.N_ID_CLIENTE) JOIN GDD_15.PUBLICACIONES P ON (CO.N_ID_PUBLICACION = P.N_ID_PUBLICACION) WHERE CL.N_ID_USUARIO = 98 AND N_ID_COMPRA NOT IN (SELECT N_ID_COMPRA FROM GDD_15.CALIFICACIONES WHERE N_ID_CLIENTE = 98 AND N_ID_COMPRA IS NOT NULL)
UNION ALL SELECT O.N_ID_PUBLICACION 'C�digo Publicaci�n', N_ID_OFERTA 'C�digo Compra u Oferta',  'Subasta' Tipo, P.D_DESCRED Descripci�n, N_MONTO 'Monto ($)', '1' Cantidad, C_ENVIO Env�or, F_ALTA 'Fecha Operaci�n' FROM GDD_15.CLIENTES CL JOIN GDD_15.OFERTAS O ON (CL.N_ID_USUARIO = O.N_ID_CLIENTE) JOIN GDD_15.PUBLICACIONES P ON (O.N_ID_PUBLICACION = P.N_ID_PUBLICACION) WHERE C_GANADOR = 'SI' AND CL.N_ID_USUARIO = 98 AND N_ID_OFERTA NOT IN (SELECT N_ID_OFERTA FROM GDD_15.CALIFICACIONES WHERE N_ID_CLIENTE = 98 AND N_ID_OFERTA IS NOT NULL)
) SQ

SELECT N_ID_COMPRA 'C�digo Compra u Oferta', (CASE WHEN C_CALIFICACION = '1' THEN '?') 'Estrellas',  FROM GDD_15.CALIFICACIONES WHERE N_ID_COMPRA IS NOT NULL
UNION ALL SELECT N_ID_OFERTA 'C�digo Compra u Oferta' FROM GDD_15.CALIFICACIONES WHERE N_ID_OFERTA IS NOT NULL



