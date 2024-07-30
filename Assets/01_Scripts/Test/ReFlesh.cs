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
            Debug.Log("��� ���");
        }
    }

    public class Temp2
    {
        public string name;

        public int currentHP;
        public float currentMP;

        public void Func()
        {
            Debug.Log("��� ���");
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
            Debug.Log("Ÿ�� �̸�: " + temp); // Ŭ������ ���� �����̽� ������ ��ġ(�ڵ� GetType()�� ����Ǵ� ��)
            Debug.Log("Ÿ�� �̸�: " + temp.GetType()); // Ŭ������ ���� �����̽� ������ ��ġ
            Debug.Log("Ÿ�� �̸�: " + temp.GetType().Name); // Ŭ������ �̸����� ������ �´�.


            // type ===================================
            DebugInValuesInfo(tempType);
        }

        private void DebugInValuesInfo(Type type)
        {
            Debug.Log("Ÿ�� �̸�: " + type);

            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                Debug.Log("��� ������ �̸� : " + field.Name);
                Debug.Log(type.GetField(field.Name));
            }// �迭�� �̷���� TempŬ���� ���� ��� �������� �α׷� Ȯ���غ���

            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                Debug.Log("������Ƽ �̸� : " + property.Name);
                Debug.Log(type.GetProperty(property.Name));
            }// �迭�� �̷���� TempŬ���� ���� 

            MethodInfo[] methos = type.GetMethods();
            foreach (MethodInfo method in methos)
            {
                Debug.Log("�޼ҵ��� �̸� : " + method.Name);
                Debug.Log(type.GetMethod(method.Name));
            }// �迭�� �̷���� TempŬ���� ���� ��� �Լ����� �α׷� Ȯ���غ���
        }
    }
}