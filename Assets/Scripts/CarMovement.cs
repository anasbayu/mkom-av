using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CarMovement : MonoBehaviour
{
    public enum ControlMode
    {
        Keyboard,
        Buttons
    };

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public ControlMode control;

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;

    // private CarLights carLights;

    void Start(){
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;

        // carLights = GetComponent<CarLights>();
    }

    // void Update()
    // {
    //     GetInputs();
    //     AnimateWheels();
    //     WheelEffects();
    // }

    void LateUpdate(){
        Move();
        Steer();
        Brake();
    }

    public void MoveInput(float input){
        moveInput = input;
    }

    public void SteerInput(float input){
        steerInput = input;
    }

    void GetInputs(){
        if(control == ControlMode.Keyboard){
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
    }

    public void GetInputs(float AImoveInput, float AIsteerInput){
        moveInput = AImoveInput;
        steerInput = AIsteerInput;

        AnimateWheels();
        // WheelEffects();
    }

    public void StopVehicle(){
        moveInput = 0;
        steerInput = 0;
        foreach (var wheel in wheels){
            wheel.wheelCollider.motorTorque = 0;
        }

        foreach (var wheel in wheels){
            wheel.wheelCollider.brakeTorque = 1000;
        }

        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void Move(){
        foreach(var wheel in wheels){
            wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
        }
    }

    void Steer(){
        foreach(var wheel in wheels){
            if (wheel.axel == Axel.Front){
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void Brake(){
        if (Input.GetKey(KeyCode.Space) || moveInput == 0){
            foreach (var wheel in wheels){
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }

            // carLights.isBackLightOn = true;
            // carLights.OperateBackLights();
        }
        else{
            foreach (var wheel in wheels){
                wheel.wheelCollider.brakeTorque = 0;
            }

            // carLights.isBackLightOn = false;
            // carLights.OperateBackLights();
        }
    }

    void AnimateWheels(){
        foreach(var wheel in wheels){
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    void WheelEffects(){
        foreach (var wheel in wheels){
            //var dirtParticleMainSettings = wheel.smokeParticle.main;

            if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude >= 10.0f){
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                wheel.smokeParticle.Emit(1);
            }else{
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }
}
