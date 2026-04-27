namespace LogicaAplicacion.InterfacesCasoDeUso.Usuarios;

public interface ILogOutUsuario
{
    // Recibe el Id del usuario que se esta deslogueando.
    // La limpieza de la variable de sesion la hace el controller (es un detalle web).
    // Este CU queda como punto de extension para, mas adelante, registrar la
    // auditoria de cierres de sesion sin tocar presentacion.
    void LogOut(int idUsuario);
}
