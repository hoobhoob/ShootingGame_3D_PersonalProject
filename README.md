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
- 총알을 ObjectPool 로 관리
<details>
<summary>오류</summary>

- 과녁판 Collider 오류로 점수가 계속 올라감.
</details>

---

# 3. 플레이 화면

**🔽 메인 화면**

![MainScene01](/Screenshots/MainScene01.png)
---

# 4. 문제점 및 해결

## 1. Cinemachine
- Cinemachine의 Virtual Camera는 별도의 Input Setting 없이 Aim을 POV 로 하면 마우스 움직임으로 카메라를 움직일 수 있다.
- 문제점 : 카메라를 움직일 때 플레이어도 같이 회전할 수 있게 방향을 알아야 하는데 Virtual Camera 의 Transform 방향은 그대로다.
- 해결 : 
