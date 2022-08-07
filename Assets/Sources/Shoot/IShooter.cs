namespace Sources.Shoot
{
    public interface IShooter
    {
        void OnHitOther(IHitTaker other);
    }
}