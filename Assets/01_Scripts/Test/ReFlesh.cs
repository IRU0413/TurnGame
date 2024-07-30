using System;
using System.Reflection;
using UnityEngine;

namespace Scripts.Test
{
    public class Temp
    {
        public int tempValue;
        public int TempValue
        {

            get { return tempValue; }
            set { tempValue = value; }
        }

        public void Func()
        {
            Debug.Log("어떠한 기능");
        }
    }

    public class Temp2
    {
        public string name;

        public int currentHP;
        public float currentMP;

        public void Func()
        {
            Debug.Log("어떠한 기능");
        }
    }
    public class ReFlesh : MonoBehaviour
    {
        private void Start()
        {
            Temp2 temp = new Temp2();
            Type tempType;

            tempType = temp.GetType();

            // temp ===================================
            temp.Func();
            Debug.Log("타입 이름: " + temp); // 클래스의 네임 스페이스 까지의 위치(자동 GetType()이 실행되는 듯)
            Debug.Log("타입 이름: " + temp.GetType()); // 클래스의 네임 스페이스 까지의 위치
            Debug.Log("타입 이름: " + temp.GetType().Name); // 클래싀의 이름만을 가지고 온다.


            // type ===================================
            DebugInValuesInfo(tempType);
        }

        private void DebugInValuesInfo(Type type)
        {
            Debug.Log("타입 이름: " + type);

            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                Debug.Log("멤버 변수의 이름 : " + field.Name);
                Debug.Log(type.GetField(field.Name));
            }// 배열로 이루어진 Temp클래스 안의 모든 변수들을 로그로 확인해본다

            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                Debug.Log("프로퍼티 이름 : " + property.Name);
                Debug.Log(type.GetProperty(property.Name));
            }// 배열로 이루어진 Temp클래스 안의 

            MethodInfo[] methos = type.GetMethods();
            foreach (MethodInfo method in methos)
            {
                Debug.Log("메소드의 이름 : " + method.Name);
                Debug.Log(type.GetMethod(method.Name));
            }// 배열로 이루어진 Temp클래스 안의 모든 함수들을 로그로 확인해본다
        }
    }
}