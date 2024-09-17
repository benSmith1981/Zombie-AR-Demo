using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location References")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destroy the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

    [Header("XR Controller References")]
    [SerializeField] private XRNode inputSource; // Specify LeftHand or RightHand
    private AudioSource audioSource; // Reference to the zombie's AudioSource

    private InputDevice _device;
    private bool _hasFired;
    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        // Get the XR device for the specified input source
        _device = InputDevices.GetDeviceAtXRNode(inputSource);
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        _hasFired = false;

    }

    void Update()
    {
        // Update the XR device if necessary
        if (!_device.isValid)
        {
            _device = InputDevices.GetDeviceAtXRNode(inputSource);
        }

        // Check if the trigger button is pressed
        if (_device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed) && isPressed)
        {
            if (!_hasFired) // Check if the gun hasn't already fired
            {
                // Calls animation on the gun that has the relevant animation events that will fire
                gunAnimator.SetTrigger("Fire");

                // Call the shoot and casing release methods
                Shoot();
                CasingRelease();

                _hasFired = true; // Set the flag to indicate the gun has fired
            }
        }
        else
        {
            // Reset the flag when the trigger is released
            _hasFired = false;
        }
    }

    // This function creates the bullet behavior
    void Shoot()
    {
        if (muzzleFlashPrefab)
        {
            // Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            // Stop the zombie's audio
            if (audioSource != null)
            {
                audioSource.Play();
            }
            // Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        // Cancel if there's no bullet prefab
        if (!bulletPrefab)
        {
            return;
        }

        // Create a bullet and add force on it in the direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation)
            .GetComponent<Rigidbody>()
            .AddForce(barrelLocation.forward * shotPower);
    }

    // This function creates a casing at the ejection slot
    void CasingRelease()
    {
        // Cancel function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        {
            return;
        }

        // Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;

        // Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(
            Random.Range(ejectPower * 0.7f, ejectPower),
            (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f),
            1f
        );

        // Add torque to make casing spin in a random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(
            new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)),
            ForceMode.Impulse
        );

        // Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }
}
