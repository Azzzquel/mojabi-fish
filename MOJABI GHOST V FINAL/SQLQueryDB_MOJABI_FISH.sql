create database DB_mojabi_fish
use DB_mojabi_fish


CREATE TABLE TB_perfiles (
    ID_Usuario int identity(1,1) PRIMARY KEY,
    Nombre varchar(255) NOT NULL UNIQUE,
    Fecha_Registro date DEFAULT GETDATE()
)

CREATE TABLE TB_partidas (
    ID_Partida int identity(1,1) PRIMARY KEY,
    ID_Ganador int NOT NULL,
    ID_Perdedor int NOT NULL,
    Fecha date DEFAULT GETDATE(),
    FOREIGN KEY (ID_Ganador) REFERENCES TB_perfiles(ID_Usuario),
    FOREIGN KEY (ID_Perdedor) REFERENCES TB_perfiles(ID_Usuario)
)

CREATE TABLE TB_rankeds (
    ID_Usuario INT PRIMARY KEY,
    Puntos_Totales INT DEFAULT 0,
    Victorias INT DEFAULT 0,
    Derrotas INT DEFAULT 0,
    FOREIGN KEY (ID_Usuario) REFERENCES TB_perfiles(ID_Usuario)
)

INSERT INTO TB_perfiles (Nombre) VALUES ('ana'), ('lalo'), ('guille');

select * from TB_perfiles;

select * from TB_partidas;

SELECT 
    p.ID_Partida,
    G.Nombre AS Ganador,
    L.Nombre AS Perdedor,
    p.Fecha
FROM TB_partidas p
JOIN TB_perfiles G ON p.ID_Ganador = G.ID_Usuario
JOIN TB_perfiles L ON p.ID_Perdedor = L.ID_Usuario;




