namespace Sources.Shoot
{
    public interface IHitTaker
    {
        void Accept(IHitTakerVisitor visitor);
    }
}