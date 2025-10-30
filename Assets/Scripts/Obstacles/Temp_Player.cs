using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Player : MonoBehaviour
{
    public float speed = 5.0f;

    void Update()
    {
        // 'Horizontal' 축 입력 값 (왼쪽/오른쪽 방향키)
        float horizontalInput = Input.GetAxis("Horizontal");

        // 'Vertical' 축 입력 값 (위/아래 방향키)
        float verticalInput = Input.GetAxis("Vertical");

        // 입력 값에 따라 이동 방향 벡터 생성
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);

        // 캐릭터 이동
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
