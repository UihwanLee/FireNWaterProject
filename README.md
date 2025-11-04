# FireNWaterProject

## Fire&Water 레퍼런스 기반 2D 게임 프로젝트
<br>


<img width="2021" height="1136" alt="image 81" src="https://github.com/user-attachments/assets/ea448543-0979-454f-89f8-619c5856eacf" />


## 1. 프로젝트 소개

Fire&Water 게임을 레퍼런스하여 만든 2D 플랫포머 게임입니다.
다음과 같은 기능을 구현하였습니다.

 - 캐릭터 이동 구현 및 상호작용
 - 협력 기반 퍼즐 요소
 - TileMap 맵 설계 및 UI 구성
 - 점수, 맵 선택 등 다양한 게임 시스템


## 2. 팀원 구성 및 역할

|Leader|Team1|Team2|Team3|Team4|
|:-----:|:-----:|:-----:|:-----:|:-----:|
|![team1](https://avatars.githubusercontent.com/u/229876104?v=4)|![team2](https://avatars.githubusercontent.com/u/233680670?v=4)|![team3](https://avatars.githubusercontent.com/u/232382009?v=4)|![team4](https://github.com/user-attachments/assets/347814aa-0400-4a21-834c-057079182127)|![team5](https://private-user-images.githubusercontent.com/36596037/320210505-fc5c2073-fd68-4d21-b52f-83a9fb6dc5f4.png?jwt=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NjIxNzg3MjMsIm5iZiI6MTc2MjE3ODQyMywicGF0aCI6Ii8zNjU5NjAzNy8zMjAyMTA1MDUtZmM1YzIwNzMtZmQ2OC00ZDIxLWI1MmYtODNhOWZiNmRjNWY0LnBuZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFWQ09EWUxTQTUzUFFLNFpBJTJGMjAyNTExMDMlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjUxMTAzVDE0MDAyM1omWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTFhYjkwZDk4YjQzMmMyYzZjMWQwYWI1YTVjYWRlM2NhNGVhMjNkYTAyNTY5MmQ4YjQ1NDdiOTZmMjhkNjYyMjUmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0In0.FFnyCdyOKG6gnjY0fBCuZy_4c52pnaJq5-7zuBbp_tg)|
|차동욱(https://github.com/lachy75127873-oss)|박재아(https://github.com/jaeapark)|장현우(https://github.com/evanjn)|신주은(https://github.com/shin0112)|이의환(https://github.com/UihwanLee)|

- 차동욱(리더): 장애물 및 퍼즐 기믹 구현
- 박재아: 캐릭터 디자인 및 Intro/Ending Scene 제작 및 관리
- 장현우: 캐릭터 디자인 및 UI 관리
- 신주은: 스테이지 플로우 및 게임 시스템 관리
- 이의환: 캐릭터 움직임 관리

## 3. 기술적 이슈

### 1) 스테이지 관리

<img width="796" height="618" alt="14" src="https://github.com/user-attachments/assets/06e7ed96-c159-4b69-923f-a51e10cc945a" />

<pre>
<code>
 private readonly Dictionary<GameState, GameState[]> _allowedTransitions = new()
{
    { GameState.None,  new[] { GameState.Start } },
    { GameState.Start, new[] { GameState.Play } },
    { GameState.Play,  new[] { GameState.Pause, GameState.Clear, GameState.Dead } },
    { GameState.Pause,  new[] { GameState.Start, GameState.Resume, GameState.Exit } },
    { GameState.Resume, new[] { GameState.Play } },
    { GameState.Dead,  new[] { GameState.Start, GameState.Exit } },
    { GameState.Clear, new[] { GameState.Exit, GameState.Next, GameState.Start } },
    { GameState.Exit,   new[] { GameState.None } },
    { GameState.Next,   new[] { GameState.Start } }
};
</code>
</pre>

 - FSM 기반으로 일관성 있는 상태 변경 가능
 - 딕셔너리로 상태가 변경될 수 있는지 검증

<br>

### 2) 플레이어 <-> 장애물 간의 상호작용(충돌) 감지

<img width="1061" height="499" alt="13" src="https://github.com/user-attachments/assets/8fdf1674-f5f9-46b4-8805-939480baa475" />

<pre>
<code>
public interface IObstacle
{
    public void Init();
}
</code>
</pre>

<pre>
<code>
public interface InteractWithController
{
   void Activate(BaseController bc);
}
</code>
</pre>

<pre>
<code>
public interface InteractableObstacle
{
    void Interact(bool on);
}
</code>
</pre>

 - interface를 이용, Obstacle과 Gimmik 상호작용 처리

<br>
 
### 3) 레버 경사면 이동 문제

![SlopeClimb3-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/023ba4d6-9f16-4e86-b2d4-353ea6948205)
 - Fire&Water 프로젝트 내 캐릭터가 레버 경사면을 타서 올라가는 기능을 확인
 - 실제 플레이 중, 일부 장애물 및 맵 구조에서 경사면 위를 이동해야 하는 상황 발생   
 - 플레이어가 경사면을 자연스럽게 오를 수 있는 로직(Slope Climb) 필요

<pre>
<code>
// Ray를 나가는 위치를 x는 boxCollider 양끝으로 설정, y위치는 height 만큼
Vector2 origin = new Vector2(center.x + (dir.x * halfWidth), center.y + height);
Debug.DrawRay(origin, Vector2.down * 0.1f, Color.blue);

// 정한 위치(origin)에서 법선 벡터를 구하기 위해 아래로 RayCast
RaycastHit2D raycastHit = Physics2D.Raycast(origin, Vector2.down, 0.4f, Define.LayerMask.SLOPE);

// 법선 벡터는 raycatsHit.normal로 구할 수 있음
Vector2 normal = raycastHit.normal;
Debug.DrawRay(raycastHit.point, normal, Color.green);

//if(raycastHit.collider != null) Debug.Log(normal);

// 평면 벡터 구하는 함수 Perpendicular를 이용하여 정규화하여 Controller에게 전달
Vector2 slopeDir = Vector2.Perpendicular(normal).normalized;
</code>
</pre>
 - Raycast를 이용, 경사면의 평면벡터를 구해서 적용하는 것으로 해결하려고 함.

<br>

### 4) TitleLogoAnmation

<img width="1123" height="507" alt="15" src="https://github.com/user-attachments/assets/40545f7e-2b07-449d-9766-c116fc07ca68" />

 - 로고->버튼이 순차적으로 FadeIn 되고 멈추는 애니메이션을 구현하려 했으나 버튼 FadeIn이 시작되면 로고가 사라지고 다시 로고 FadeIn부터 반복되는 문제상황 발생
 - Exit 연결을 해제해봤지만 로고 애니메이션의 Loop오류 동일 -> Animation 하나에 로고/버튼애니메이션을 모두 넣어서 해결

<br>


## 4. 시스템 구성도

> 와이드 프레임
  <img width="989" height="828" alt="1" src="https://github.com/user-attachments/assets/52bb5c2a-eef0-4eeb-9ee4-7cb51da3ddda" />

> Stage 상태 머신(State Machine)
  <img width="916" height="625" alt="2" src="https://github.com/user-attachments/assets/27dec39c-33b0-43fa-b8e7-7a9e7de48e96" />

> 리소스 관리
<img width="523" height="744" alt="3" src="https://github.com/user-attachments/assets/fc3fcbbe-8899-40ad-9f4b-57eccbb17fcf" />




