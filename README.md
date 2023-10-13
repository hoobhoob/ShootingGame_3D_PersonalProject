# ShootingGame_3D_PersonalProject - Unity B06조 장성림

### 목차

1. 게임 설명
2. 사용한 기능
3. 플레이 화면
4. 문제점 및 해결
---

# 1. 게임 설명

- **게임명: `3DShootingGame_Practice`**

- **담당자 : Unity B06조 장성림**

- **설명:** [내일 배움 캠프 8기 Unity] Unity 게임개발 심화 개인과제.

- **개요:**
    - 슈팅 게임의 플레이어를 구현.

---

# 2. 사용한 기능

- Cinemachine 의 가상 카메라
- 믹사모 캐릭터 모델 Import
- 믹사모 애니메이션 Import
- 애니메이션 Blend tree
- 총알 발사를 투사체로 구현
- 총알 발사 이펙트를 Particle System 로 직접 구현
- 총알을 ObjectPool 로 관리
<details>
<summary>오류</summary>

- 과녁판 Collider 오류로 점수가 계속 올라감.
</details>

---

# 3. 플레이 화면

**🔽 메인 화면**

플레이 화면

![MainScene01](/Screenshots/MainScene01.png)

총알 발사 이펙트

![BulletEffect01](/Screenshots/BulletEffect01.png)
---

# 4. 문제점 및 해결

## 1. Cinemachine
- Cinemachine의 Virtual Camera는 별도의 Input Setting 없이 Aim을 POV 로 하면 마우스 움직임으로 카메라를 움직일 수 있다.
- 문제점
    - 카메라를 움직일 때 플레이어도 같이 회전할 수 있게 방향을 알아야 하는데 Virtual Camera 의 Transform 방향은 그대로다.
- 해결
    - Virtual Camera 의 컴퍼넌트인 CinemachinePOV를 GetCinemachineComponent<> 로 받아서 그것의 회전 값인 m_VerticalAxis.Value 와 m_HorizontalAxis.Value 를 가지고 forward를 구했다.


## 2. 믹사모 캐릭터 및 애니메이션 Import
- 문제점
    - 캐릭터와 애니메이션을 Import 하면 Material 과 캐릭터, 애니메이션 세팅을 해야 한다.
- 해결
    - 캐릭터 모델의 Rig 를 Humanoid로 하고 Material을 export 하면 인식이 잘 된다.
    - 애니메이션도 Rig를 Humanoid로 하고 아바타를 캐릭터 모델의 아바타로 설정 한다.


## 3. 애니메이션 컨트롤러
- 문제점
    - 앞으로 움직이기, 대각선으로 움직이기, 옆으로, 뒤 대각선, 뒤 등 다양한 움직임 애니메이션이 있는데 하나씩 만들어서 방향을 설정하면서 트랜지션을 하기에는 너무 복잡해진다.
- 해결
    - Blend Tree 를 만들어서 설정한 Float 값 2개 (방향 벡터로 생각) 에 따라 애니메이션을 쉽게 바꿀 수 있다.

## 4. 총알을 투사체로 구현
- 문제점
    - 총알 오브젝트의 속도를 실제 총알 속도처럼 구현하려면 너무 빠르게 움직여서 충돌 처리가 잘 안된다.
- 해결
    - 총알 Collider 크기를 아래 그림처럼 실제 오브젝트 크기보다 길게 설정해서 총알이 빠르게 움직여도 Colllider는 길게 남아있어 이미 오브젝트는 총알을 맞았다 는 느낌으로 충돌 처리를 했다.

![BulletCollider01](/Screenshots/BulletCollider01.png)

![BulletCollider02](/Screenshots/BulletCollider02.png)

## 5. 과녁판의 Collider - 아직 해결 못함
- 문제점
    - 각 원판을 만들고 Collider를 추가해서 스크립트에 각 원판들의 Collider를 배열로 받아 IsTrigger 로 충돌 처리를 하려고 했으나 게임 시작부터 계속 true 라서 생각과는 달랐다.
- 해결중
    - 처음에는 Mesh Collider로 되어있어서 원판끼리 겹치는 부분이 있어서 그랬을 수도 있다는 생각에 Collider를 2D로 바꾸고 원판끼리 거리를 넓혀보았다. 그래도 안됬다.
    - 아마도 IsTrigger는 Collider의 IsTrigger 체크 표시가 되어있나를 물어보는 것 같아서 찾아보는 중이다.
