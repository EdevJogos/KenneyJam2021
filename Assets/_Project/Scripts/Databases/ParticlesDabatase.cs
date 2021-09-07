using UnityEngine;

public class ParticlesDabatase : MonoBehaviour
{
    [System.Serializable]
    public struct ParticleData
    {
        public Particles ID;
        public GameObject[] particles;
    }

    public static ParticlesDabatase Instance;

    public ParticleData[] particles;

    private void Awake()
    {
        Instance = this;
    }

    public static void InstantiateParticle(Particles p_id, int p_index, Vector2 p_position)
    {
        GameObject __particlePrefab = GetParticle(p_id, p_index);

        Instantiate(__particlePrefab, p_position, __particlePrefab.transform.rotation);
    }

    public static void InstantiateParticle(Particles p_id, int p_index, Vector2 p_position, Quaternion p_rotation)
    {
        GameObject __particlePrefab = GetParticle(p_id, p_index);

        Instantiate(__particlePrefab, p_position, p_rotation);
    }

    public static GameObject InstantiateParticle(Particles p_id, int p_index, Transform p_parent)
    {
        return Instantiate(GetParticle(p_id, p_index), p_parent);
    }

    private static GameObject GetParticle(Particles p_id, int p_index)
    {
        for (int __i = 0; __i < Instance.particles.Length; __i++)
        {
            if (Instance.particles[__i].ID == p_id)
            {
                return Instance.particles[__i].particles[p_index];
            }
        }

        return null;
    }
}
