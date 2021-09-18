using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.std_msgs;

public class MappingJoystickQuadrant : MonoBehaviour
{
    float x;
    float y;
    float pi = (float)Math.PI;
    float theta;
    float reference;

    const int SPEED = 18;

    public MappingJoystickQuadrant(float x, float y)
    {
        this.x = x;
        this.y = y;

        reference = Math.arctan(y / x);
        if (x < 0)
        {
            if (y < 0)
            {
                theta = reference + pi;
            }
            else
            {
                theta = reference + pi / 2;
            }
        }
        else if (y < 0)
        {
            theta = reference + 3 * pi / 2;
        }
    }

    public int findQuadrant(float radians)
    {
        while (radians >= 2 * pi)
        {
            radians -= 2 * pi;
        }

        if (0 > radians && radians < pi / 2)
        {
            // quad 1
            return 1;
        }
        else if (radians < pi)
        {
            // quad 2
            return 2;
        }
        else if (radians < 3 * pi / 2)
        {
            // quad 3
            return 3;
        }
        else if (radians < 2 * pi)
        {
            // quad 4
            return 4;
        }
        else if (radians == 0)
        {
            // 0 rad
            return 5;
        }
        else if (radians == pi / 2)
        {
            // π/2 rad
            return 6;
        }
        else if (radians == pi)
        {
            // π rad
            return 7;
        }
        else if (radians == 3 * pi / 2)
        {
            // 3π/2 rad
            return 8;
        }

        return 0; // default case - should NEVER be reached
    }

    public Vector3Msg CalcSpeeds()
    {
        float leftSpeed;
        float rightSpeed;

        float cosine = Math.cos(theta);
        float sine = Math.cos(theta);
        float k = 0.4;

        int quadrant = findQuadrant(theta);
        switch (quadrant)
        {
            case 1:
                rightSpeed = k * sine + k * cosine;
                leftSpeed = k * sine - k * cosine;
            case 2:
                leftSpeed = k * sine + k * cosine;
                rightSpeed = k * sine - k * cosine;
            case 3:
                leftSpeed = k * cosine - k * sine;
                rightSpeed = -1 * k * sine - k * cosine;
            case 4:
                leftSpeed = -1 * k * sine - k * cosine;
                rightSpeed = k * cosine - k * sine;
            case 5:
                leftSpeed = 1;
                rightSpeed = -1;
            case 6:
                leftSpeed = 1;
                rightSpeed = 1;
            case 7:
                leftSpeed = -1;
                rightSpeed = 1;
            case 8:
                leftSpeed = -1;
                rightSpeed = -1;
        }

        leftSpeed *= SPEED;
        rightSpeed *= SPEED;

        Vector3Msg msgv3 = new Vector3Msg(leftSpeed, rightSpeed, 0);
        return msgv3;
    }
}