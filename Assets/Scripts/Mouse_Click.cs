using UnityEngine;

/*Mouse_Click
마우스 클릭으로 활성화 시킬 수 있는 것들을 모아둔 스크립트
----------------------------------
Update  클릭 처리 : 3D상에서 클릭으로 활성화 시킬 수 있는 행동을 처리하는 코드
3D상에서 클릭으로 활성화 할 수 있는 행동 : 
포탈 -
 */
public class Mouse_Click : MonoBehaviour
{
    public DisPatch_UI disPatchUI;
    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //메인 카메라로 부터 클릭한 위치 획득
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 0f);

            //위에서 받은 위치로 부터 레이캐스트
            if (hit.collider != null)
            {
                //포탈 검출 시 포탈 UI활성화 및 정보 전달
                if (hit.collider.CompareTag("Portal"))
                {
                    Debug.Log("포탈 검출");
                    disPatchUI.gameObject.SetActive(true);
                    Portal targetportal = hit.collider.gameObject.GetComponent<Portal>();
                    disPatchUI.Setting_UI(targetportal);
                    targetportal.GetComponent<DisPatch>().SetDis_UI(disPatchUI);
                }
            }
        }
    }
}
