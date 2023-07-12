using UnityEngine;

namespace FiveRabbitsDemo
{
    public class AnimatorParamatersChange : MonoBehaviour
    {

        private string[] m_buttonNames = new string[] { "Idle", "Run", "Dead" };

        private Animator m_animator;

        private bool idling = true;

        // Use this for initialization
        void Start()
        {

            m_animator = GetComponent<Animator>();

        }

        // Update is called once per frame
        void Update()
        {
            float rand = Random.Range(0.0f, 100.0f);
            if (idling && rand < 10.0f)
            {
                m_animator.SetInteger("AnimIndex", 1);
                idling = false;
            }
            else if (!idling)
            {
                
            }
        }

       /* private void OnGUI()
        {
            GUI.BeginGroup(new Rect(0, 0, 150, 1000));

            for (int i = 0; i < m_buttonNames.Length; i++)
            {
                if (GUILayout.Button(m_buttonNames[i], GUILayout.Width(150)))
                {
                    m_animator.SetInteger("AnimIndex", i);
                    m_animator.SetTrigger("Next");
                }
            }

            GUI.EndGroup();
        }*/
    }
}
