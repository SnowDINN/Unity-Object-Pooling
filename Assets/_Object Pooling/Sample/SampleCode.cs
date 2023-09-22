using Anonymous.Pooling;
using UnityEngine;

public class SampleCode : MonoBehaviour
{
    private GameObject one, two;
    
    private void Awake()
    {
        one = ObjectPooling.Default.Rent("Cube");
        two = ObjectPooling.Default.Rent("Cube");
        
        Invoke(nameof(Disable), 5.0f);
        Invoke(nameof(Enable), 10.0f);
        Invoke(nameof(Disable), 15.0f);
    }

    private void Enable()
    {
        one = ObjectPooling.Default.Rent("Cube");
        two = ObjectPooling.Default.Rent("Cube");
    }

    private void Disable()
    {
        ObjectPooling.Default.Return(one);
        ObjectPooling.Default.Return(two);
    }
}