/* Hibás operációknál a visszaadott érték.
 * ErrorObject-tel végzett összes művelet ErrorObject-et ad vissza.
 * Kezelése könnyebb mint a null-lé.
 */ 
namespace _3D_layout_script
{
    public class ErrorObject
    {
        public static implicit operator string (ErrorObject eo)
        {
            return "ErrorType";
        }

        public override string ToString()
        {
            return "ErrorType";
        }
    }
}
