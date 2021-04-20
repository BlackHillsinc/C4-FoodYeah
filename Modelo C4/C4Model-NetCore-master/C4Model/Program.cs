﻿using Structurizr;
using Structurizr.Api;
using System.Linq;

namespace C4Model
{
    class Program
    {
        static void Main(string[] args)
        {
            FoodYeah();
        }

        static void FoodYeah()
        {
            const long workspaceId = 65073;
            const string apiKey = "66c60d2c-9460-4e30-bba7-d15c7b6d88b1";
            const string apiSecret = "36618a8e-97ba-455d-8a3f-c39be7c9fbc5";
            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("FoodYeah", "FoodYeah - C4 Model");
            Model model = workspace.Model;

           
            /////////////////
            SoftwareSystem appSystem = model.AddSoftwareSystem("FoodYeah", "Servicio de FoodYeah");
            SoftwareSystem VisaApi = model.AddSoftwareSystem("Visa", "Servicio para transacciones por internet");
            SoftwareSystem Socrates = model.AddSoftwareSystem("Socrates", "Servicio para almacenar información de estudiantes y profesores");
            ///////////////////

            
            /////////////////////
            Person customer = model.AddPerson("Customer", "Cliente de la Upc");
            /////////////////////

            
            ///////////////////
            appSystem.AddTags("FoodYeah");
            VisaApi.AddTags("Visa");
            Socrates.AddTags("Socrates");
            /////////////////////

            
            ////////////
            customer.Uses(appSystem, "Usa");
            ////////////

            
            ////////////////
            appSystem.Uses(VisaApi, "Gets the sender account and the recipient's account in and Account funding Transaction");
            appSystem.Uses(Socrates, "Gets email information for login authentication");
            ///////////////

            

            ViewSet viewSet = workspace.Views;

            // 1. Diagrama de Contexto
            
            SystemContextView contextFYView = viewSet.CreateSystemContextView(appSystem, "ContextoFY", "Diagrama de contexto - FoodYeah");
            contextFYView.PaperSize = PaperSize.A4_Landscape;
            contextFYView.AddAllSoftwareSystems();
            contextFYView.AddAllPeople();
           
            
            Styles styles = viewSet.Configuration.Styles;

            //////////////////////
            styles.Add(new ElementStyle(Tags.Person) { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("FoodYeah") { Background = "#0202FA", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Visa") { Background = "#0202FA", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Socrates") { Background = "#515151", Color = "#ffffff", Shape = Shape.RoundedBox });
            //////////////////////

            // 2. Diagrama de Contenedores
            
            ////////////////////////
            Container WebApplication = appSystem.AddContainer("Aplicación Web", "Deliver the static content and the FoodYeah single page Application", "Net Core,nginx port 80 ");
            Container DataBase = appSystem.AddContainer("Base de datos", "Stores Users registrations,information,etc","Sql server");
            Container FoodYeahApi = appSystem.AddContainer("Restful api", "Provides all the foodyeah funcionality via Json and HTTPS api"," Net Core,nginx port 80");
            Container SinglePage = appSystem.AddContainer("Single Page Application", "Provides all the foodyeah funcionality via web browser", "Net Core");
            ///////////////////////
           
            ////////////////////
            customer.Uses(WebApplication, "Usa", "Visits Using FoodYeah.com");
            customer.Uses(SinglePage, "Usa", "Buys and gets information about the food in cafeteria ");
            FoodYeahApi.Uses(Socrates, "Usa", "Gets email information for login autentication");
            FoodYeahApi.Uses(VisaApi, "Usa", "Gets the sender account and the recipient's account in and Account funding Transaction");
            WebApplication.AddTags("webApp");
            FoodYeahApi.AddTags("APIFY");
            DataBase.AddTags("DB");
            SinglePage.AddTags("Landing");
            WebApplication.Uses(SinglePage, "Usa", "Delivers to the customer web's browser");
            SinglePage.Uses(FoodYeahApi, "Makes Apis Calls", "Https/Json");
            FoodYeahApi.Uses(DataBase, "Usa", "Reads from and writes to");
            ///////////////////

           

           
            //////////////////
            styles.Add(new ElementStyle("webApp"){Background= "#07059B",Color="#ffffff",Shape=Shape.WebBrowser,Icon= "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTI1IiBoZWlnaHQ9Ijc4IiB2aWV3Qm94PSIwIDAgMTI1IDc4IiB2ZXJzaW9uPSIxLjEiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiPg0KPHRpdGxlPm5ldGNvcmU8L3RpdGxlPg0KPGRlc2M+Q3JlYXRlZCB1c2luZyBGaWdtYTwvZGVzYz4NCjxnIGlkPSJDYW52YXMiIHRyYW5zZm9ybT0idHJhbnNsYXRlKC0zNjE4IC00MTQpIj4NCjxnIGlkPSJuZXRjb3JlIj4NCjxnIGlkPSJWZWN0b3IiPg0KPHVzZSB4bGluazpocmVmPSIjcGF0aDBfZmlsbCIgdHJhbnNmb3JtPSJ0cmFuc2xhdGUoMzYxOCA0NTMuMTcpIiBmaWxsPSIjNjIxRUU1Ii8+DQo8L2c+DQo8ZyBpZD0iVmVjdG9yIj4NCjx1c2UgeGxpbms6aHJlZj0iI3BhdGgxX2ZpbGwiIHRyYW5zZm9ybT0idHJhbnNsYXRlKDM2MzQuNSA0MTQpIiBmaWxsPSIjNjIxRUU1Ii8+DQo8L2c+DQo8ZyBpZD0iVmVjdG9yIj4NCjx1c2UgeGxpbms6aHJlZj0iI3BhdGgyX2ZpbGwiIHRyYW5zZm9ybT0idHJhbnNsYXRlKDM2ODMuMDEgNDE0KSIgZmlsbD0iIzYyMUVFNSIvPg0KPC9nPg0KPGcgaWQ9IlZlY3RvciI+DQo8dXNlIHhsaW5rOmhyZWY9IiNwYXRoM19maWxsIiB0cmFuc2Zvcm09InRyYW5zbGF0ZSgzNzExLjMzIDQxNCkiIGZpbGw9IiM2MjFFRTUiLz4NCjwvZz4NCjxnIGlkPSJWZWN0b3IiPg0KPHVzZSB4bGluazpocmVmPSIjcGF0aDRfZmlsbCIgdHJhbnNmb3JtPSJ0cmFuc2xhdGUoMzY1Mi42MyA0NzAuMzA1KSIgZmlsbD0iIzYyMUVFNSIvPg0KPC9nPg0KPGcgaWQ9IlZlY3RvciI+DQo8dXNlIHhsaW5rOmhyZWY9IiNwYXRoNV9maWxsIiB0cmFuc2Zvcm09InRyYW5zbGF0ZSgzNjcxLjIzIDQ3Ni4xMTcpIiBmaWxsPSIjNjIxRUU1Ii8+DQo8L2c+DQo8ZyBpZD0iVmVjdG9yIj4NCjx1c2UgeGxpbms6aHJlZj0iI3BhdGg2X2ZpbGwiIHRyYW5zZm9ybT0idHJhbnNsYXRlKDM2ODkuODEgNDc2LjM0OCkiIGZpbGw9IiM2MjFFRTUiLz4NCjwvZz4NCjxnIGlkPSJWZWN0b3IiPg0KPHVzZSB4bGluazpocmVmPSIjcGF0aDdfZmlsbCIgdHJhbnNmb3JtPSJ0cmFuc2xhdGUoMzY5OS4xNCA0NzYuMjE4KSIgZmlsbD0iIzYyMUVFNSIvPg0KPC9nPg0KPC9nPg0KPC9nPg0KPGRlZnM+DQo8cGF0aCBpZD0icGF0aDBfZmlsbCIgZD0iTSAzLjUyNDU5IDcuMTQyNjhDIDMuMDU4MDMgNy4xNTAwNSAyLjU5NSA3LjA2MDMyIDIuMTY0NzggNi44NzkxNUMgMS43MzQ1NiA2LjY5Nzk4IDEuMzQ2NDkgNi40MjkzMSAxLjAyNTEgNi4wOTAxMkMgMC42OTQ4ODYgNS43NjMwMiAwLjQzMzkxOSA1LjM3MjQ1IDAuMjU3ODI5IDQuOTQxODNDIDAuMDgxNzM4NSA0LjUxMTIgLTAuMDA1ODUyNjggNC4wNDkzNyAwLjAwMDMwMjc0IDMuNTg0QyAtMC4wMDMwMzEzMSAzLjExOTAyIDAuMDg1ODQ5OSAyLjY1ODAyIDAuMjYxNzc5IDIuMjI3NzlDIDAuNDM3NzA3IDEuNzk3NTYgMC42OTcxNzUgMS40MDY2OCAxLjAyNTEgMS4wNzc4OEMgMS4zNDMzNCAwLjczMjkxNyAxLjczMDI0IDAuNDU4NzUzIDIuMTYwNzkgMC4yNzMxMjVDIDIuNTkxMzUgMC4wODc0OTc4IDMuMDU1OTYgLTAuMDA1NDYxMzkgMy41MjQ1OSAwLjAwMDI1MjA4NEMgMy45OTI1NSAtMC4wMDA4OTU4MjMgNC40NTU3NyAwLjA5NDEyOCA0Ljg4NTY3IDAuMjc5NDczQyA1LjMxNTU3IDAuNDY0ODE5IDUuNzAzMDEgMC43MzY1NTIgNi4wMjQwOSAxLjA3Nzg4QyA2LjM1NzY3IDEuNDAzNzQgNi42MjI1IDEuNzkzNTEgNi44MDI4NCAyLjIyNDAzQyA2Ljk4MzE4IDIuNjU0NTUgNy4wNzUzNiAzLjExNzA1IDcuMDczODggMy41ODRDIDcuMDc4MjQgNC4wNTEzOCA2Ljk4NzM5IDQuNTE0NzIgNi44MDY4NyA0Ljk0NTY1QyA2LjYyNjM2IDUuMzc2NTggNi4zNTk5OSA1Ljc2NjAzIDYuMDI0MDkgNi4wOTAxMkMgNS42OTk5NyA2LjQyNTczIDUuMzExMzkgNi42OTIwMiA0Ljg4MTggNi44NzI5MkMgNC40NTIyMSA3LjA1MzgzIDMuOTkwNTMgNy4xNDU2IDMuNTI0NTkgNy4xNDI2OFoiLz4NCjxwYXRoIGlkPSJwYXRoMV9maWxsIiBkPSJNIDM2LjY5MjYgNDUuNjg2NUwgMzAuMTQzOSA0NS42ODY1TCA2LjcyMzY1IDkuMjQ3NTdDIDYuMTQwMjYgOC4zNDk3NiA1LjY1NDI3IDcuMzkxOTkgNS4yNzM5NCA2LjM5MDZMIDUuMDczOTggNi4zOTA2QyA1LjI5NjU1IDguNDcxMSA1LjM4MDA2IDEwLjU2NDIgNS4zMjM5MyAxMi42NTU5TCA1LjMyMzkzIDQ1LjY4NjVMIDMuODEzOTNlLTA3IDQ1LjY4NjVMIDMuODEzOTNlLTA3IDEuMTQ3MjFlLTA2TCA2LjkyMzYxIDEuMTQ3MjFlLTA2TCAyOS43MTkgMzUuNzg3M0MgMzAuNjY4OCAzNy4yNzQzIDMxLjI4NTQgMzguMjkzNSAzMS41Njg3IDM4Ljg0NDhMIDMxLjY5MzYgMzguODQ0OEMgMzEuNDI5OSAzNi42MTYzIDMxLjMyMTMgMzQuMzcyMSAzMS4zNjg3IDMyLjEyODRMIDMxLjM2ODcgMS4xNDcyMWUtMDZMIDM2LjY5MjYgMS4xNDcyMWUtMDZMIDM2LjY5MjYgNDUuNjg2NVoiLz4NCjxwYXRoIGlkPSJwYXRoMl9maWxsIiBkPSJNIDI0LjIyMDEgNDUuNjg2NUwgMCA0NS42ODY1TCAwIDEuMTQ3MjFlLTA2TCAyMy4wOTU0IDEuMTQ3MjFlLTA2TCAyMy4wOTU0IDQuODM2ODFMIDUuNDQ4OSA0LjgzNjgxTCA1LjQ0ODkgMjAuMDIzOUwgMjEuODk1NiAyMC4wMjM5TCAyMS44OTU2IDI0LjgzNTZMIDUuNDQ4OSAyNC44MzU2TCA1LjQ0ODkgNDAuNjc0M0wgMjQuMjQ1MSA0MC42NzQzTCAyNC4yMjAxIDQ1LjY4NjVaIi8+DQo8cGF0aCBpZD0icGF0aDNfZmlsbCIgZD0iTSAzMS42Njg2IDQuODM2ODFMIDE4LjQyMTMgNC44MzY4MUwgMTguNDIxMyA0NS42ODY1TCAxMy4wOTc0IDQ1LjY4NjVMIDEzLjA5NzQgNC44MzY4MUwgNC41NzY3MmUtMDYgNC44MzY4MUwgNC41NzY3MmUtMDYgMS4xNDcyMWUtMDZMIDMxLjY2ODYgMS4xNDcyMWUtMDZMIDMxLjY2ODYgNC44MzY4MVoiLz4NCjxwYXRoIGlkPSJwYXRoNF9maWxsIiBkPSJNIDE1LjY4MzIgMjAuNDU3MUMgMTMuODc4NiAyMS4zMzM0IDExLjg4ODEgMjEuNzU0OSA5Ljg4NDMyIDIxLjY4NTFDIDguNTUyOCAyMS43NDUzIDcuMjIzODUgMjEuNTE5MiA1Ljk4NjYxIDIxLjAyMjFDIDQuNzQ5MzcgMjAuNTI1MSAzLjYzMjQ1IDE5Ljc2ODUgMi43MTA3NyAxOC44MDMxQyAwLjg3NDIwNyAxNi43NDE4IC0wLjA5MzQ2MjggMTQuMDQ2NyAwLjAxMTMwOTYgMTEuMjg0N0MgLTAuMDU1OTIyMyA5Ljc4OTI3IDAuMTc3MTQyIDguMjk1NDkgMC42OTY2NzMgNi44OTIwNkMgMS4yMTYyIDUuNDg4NjIgMi4wMTE2MSA0LjIwNDEzIDMuMDM1NyAzLjExNDc5QyA0LjAzMzcgMi4wODk2OSA1LjIzMzUzIDEuMjg0MDMgNi41NTg4OSAwLjc0OTAyN0MgNy44ODQyNSAwLjIxNDAyNyA5LjMwNjE0IC0wLjAzODU5OTIgMTAuNzM0MiAwLjAwNzIxMjg0QyAxMi40NDMxIC0wLjA1MTM0NDEgMTQuMTQ1NiAwLjI0NzM2NiAxNS43MzMxIDAuODg0MzVMIDE1LjczMzEgMy4zOTA0N0MgMTQuMjA0NiAyLjU0MjAyIDEyLjQ4MTEgMi4xMDk5OCAxMC43MzQyIDIuMTM3NDFDIDkuNjMyODMgMi4xMDYzMSA4LjUzNzggMi4zMTQxNCA3LjUyNDAxIDIuNzQ2NjhDIDYuNTEwMjIgMy4xNzkyMiA1LjYwMTYxIDMuODI2MjYgNC44NjAzNCA0LjY0MzUzQyAzLjMwMTUxIDYuNDI0MTEgMi40OTQ3NSA4Ljc0MjkyIDIuNjEwNzggMTEuMTA5M0MgMi40OTQwNyAxMy4zNTEgMy4yNDYzOSAxNS41NTEgNC43MTAzNiAxNy4yNDkzQyA1LjQxMjU4IDE4LjAwNjQgNi4yNjk4NCAxOC42MDIyIDcuMjIzMzIgMTguOTk1OUMgOC4xNzY4MSAxOS4zODk1IDkuMjAzOTUgMTkuNTcxNyAxMC4yMzQzIDE5LjUyOTlDIDEyLjE1MzQgMTkuNTgwMiAxNC4wNDg3IDE5LjA5NDMgMTUuNzA4MiAxOC4xMjY0TCAxNS42ODMyIDIwLjQ1NzFaIi8+DQo8cGF0aCBpZD0icGF0aDVfZmlsbCIgZD0iTSA3LjMxMjIzIDE1Ljg3MzNDIDYuMzE3MjYgMTUuOTA1NiA1LjMyNjk5IDE1LjcyMzEgNC40MDg0NSAxNS4zMzgzQyAzLjQ4OTkyIDE0Ljk1MzUgMi42NjQ1OCAxNC4zNzUzIDEuOTg4MyAxMy42NDI4QyAwLjYyMTUyOSAxMi4xMTM1IC0wLjA4ODA3MDUgMTAuMTA1MSAwLjAxMzcwNCA4LjA1NDE4QyAtMC4wNDg3MTMzIDYuOTc5NjggMC4xMDI1NTEgNS45MDM0NSAwLjQ1ODcwNCA0Ljg4ODFDIDAuODE0ODU2IDMuODcyNzQgMS4zNjg3OSAyLjkzODUxIDIuMDg4MjkgMi4xMzk3NEMgMi44MjI1OCAxLjQyMTIxIDMuNjk2MTIgMC44NjE0MTIgNC42NTQ4NyAwLjQ5NTAwM0MgNS42MTM2MiAwLjEyODU5MyA2LjYzNzE3IC0wLjAzNjYzMTggNy42NjIxNyAwLjAwOTU0MTk5QyA4LjYzMTAzIC0wLjA0MjE4NzkgOS41OTk1OSAwLjExNjkzNCAxMC41MDEzIDAuNDc1OTg4QyAxMS40MDMxIDAuODM1MDQyIDEyLjIxNjYgMS4zODU1MiAxMi44ODYxIDIuMDg5NjFDIDE0LjIxNDYgMy42OTQ4NSAxNC44ODQ3IDUuNzQ2NDMgMTQuNzYwNyA3LjgyODYyQyAxNC44NjU2IDkuOTQxOCAxNC4xMzg2IDEyLjAxMTcgMTIuNzM2MSAxMy41OTI3QyAxMi4wNDcxIDE0LjMzOTQgMTEuMjA2NSAxNC45MjkzIDEwLjI3MDggMTUuMzIyN0MgOS4zMzUyMSAxNS43MTYxIDguMzI2MzQgMTUuOTAzOCA3LjMxMjIzIDE1Ljg3MzNMIDcuMzEyMjMgMTUuODczM1pNIDcuNDg3MiAyLjE2NDc5QyA2Ljc5OTY1IDIuMTM1NzggNi4xMTQ0NCAyLjI2MjY4IDUuNDgyNTkgMi41MzYwNUMgNC44NTA3NSAyLjgwOTQyIDQuMjg4NTcgMy4yMjIyIDMuODM3OTMgMy43NDM2NUMgMi44NzYxNiA0Ljk4NTM0IDIuMzk2NTYgNi41MzQ4MiAyLjQ4ODIxIDguMTA0M0MgMi40MDA5MSA5LjYyNzEgMi44ODIwNiAxMS4xMjggMy44Mzc5MyAxMi4zMTQ2QyA0LjI5MzI1IDEyLjgyODcgNC44NTY5IDEzLjIzNDggNS40ODgwNSAxMy41MDM1QyA2LjExOTIxIDEzLjc3MjIgNi44MDIwOCAxMy44OTY5IDcuNDg3MiAxMy44Njg0QyA4LjE1Nzk0IDEzLjkwODEgOC44Mjg4NCAxMy43OTMyIDkuNDQ4NDIgMTMuNTMyNUMgMTAuMDY4IDEzLjI3MTkgMTAuNjE5OCAxMi44NzI0IDExLjA2MTUgMTIuMzY0N0MgMTEuOTcwMiAxMS4xMTk3IDEyLjQxMjcgOS41OTM2MSAxMi4zMTEyIDguMDU0MThDIDEyLjQxMDEgNi41MDcyNSAxMS45NjgxIDQuOTc0MTUgMTEuMDYxNSAzLjcxODU5QyAxMC42MjU5IDMuMjAwMzEgMTAuMDc2NSAyLjc5MDI4IDkuNDU2MzkgMi41MjA2OUMgOC44MzYyNCAyLjI1MTEgOC4xNjIyMSAyLjEyOTI4IDcuNDg3MiAyLjE2NDc5TCA3LjQ4NzIgMi4xNjQ3OVoiLz4NCjxwYXRoIGlkPSJwYXRoNl9maWxsIiBkPSJNIDcuODQ4NDIgMi42ODU1MkMgNy4zMDg1NiAyLjMzMDkgNi42Njc1OCAyLjE2MzYzIDYuMDIzNzkgMi4yMDkzNUMgNS41MjAyNyAyLjIyNzcyIDUuMDI4OTYgMi4zNjk4OSA0LjU5MzExIDIuNjIzMzZDIDQuMTU3MjYgMi44NzY4MiAzLjc5MDIzIDMuMjMzODEgMy41MjQyOSAzLjY2MjlDIDIuNzgwMzYgNC44NDM4NCAyLjQyMjE0IDYuMjI3OTQgMi40OTk1IDcuNjIyNTZMIDIuNDk5NSAxNS4yOTEzTCAzLjA1MTE0ZS0wNiAxNS4yOTEzTCAzLjA1MTE0ZS0wNiAwLjI1NDU3OUwgMi40OTk1IDAuMjU0NTc5TCAyLjQ5OTUgMy4zODcyM0MgMi43NzQyOCAyLjQxNzkzIDMuMzIwODEgMS41NDgxIDQuMDc0MTggMC44ODExMDhDIDQuNzI2MDQgMC4zMTM4ODYgNS41NjA2IDAuMDAyMzI4OTEgNi40MjM3MSAwLjAwMzk3MTE5QyA2LjkwNjg4IC0wLjAxNzgwMDEgNy4zODk5IDAuMDUwMTY2NyA3Ljg0ODQyIDAuMjA0NDU2TCA3Ljg0ODQyIDIuNjg1NTJaIi8+DQo8cGF0aCBpZD0icGF0aDdfZmlsbCIgZD0iTSAxMy4xNjMgOC41Mjk2N0wgMi41OTAxMiA4LjUyOTY3QyAyLjUxOTE4IDkuOTMwNjUgMi45NzM5NCAxMS4zMDc1IDMuODY0ODYgMTIuMzg5MUMgNC4zMjgxNyAxMi44NTI0IDQuODgzOCAxMy4yMTI0IDUuNDk1NDIgMTMuNDQ1NkMgNi4xMDcwNCAxMy42Nzg4IDYuNzYwODcgMTMuNzggNy40MTQxNCAxMy43NDI0QyA5LjEwODU0IDEzLjcyNTUgMTAuNzQ3MyAxMy4xMzM2IDEyLjA2MzIgMTIuMDYzM0wgMTIuMDYzMiAxNC4zMTg4QyAxMC41MTc4IDE1LjM0MDEgOC42ODc4MSAxNS44NDA1IDYuODM5MjYgMTUuNzQ3M0MgNS45MDY1NSAxNS43OTQ1IDQuOTc0OTUgMTUuNjM0OCA0LjExMDg2IDE1LjI3OTZDIDMuMjQ2NzcgMTQuOTI0MyAyLjQ3MTQ4IDE0LjM4MjQgMS44NDAyNyAxMy42OTIzQyAwLjUzOTA0NCAxMi4wNjQ5IC0wLjExMTQgMTAuMDEwMSAwLjAxNTYzNSA3LjkyODE5QyAtMC4wNzQyMDEgNS44NDExNyAwLjYzMTgyNCAzLjc5ODE0IDEuOTkwMjQgMi4yMTQyNUMgMi42MDg0NSAxLjQ5ODIzIDMuMzc3NjYgMC45Mjg5MjIgNC4yNDIyMSAwLjU0NzUwOUMgNS4xMDY3NyAwLjE2NjA5NiA2LjA0NTA3IC0wLjAxNzg5MjYgNi45ODkyMyAwLjAwODg2NTM3QyA3Ljg0MTc2IC0wLjAzOTAyNTQgOC42OTQwMSAwLjEwNzQ4MyA5LjQ4MTkzIDAuNDM3Mzc3QyAxMC4yNjk5IDAuNzY3MjcgMTAuOTcyOSAxLjI3MTk3IDExLjUzODMgMS45MTM1MkMgMTIuNjg5MiAzLjQzNTE0IDEzLjI1NjkgNS4zMjA1MyAxMy4xMzggNy4yMjY0OUwgMTMuMTYzIDguNTI5NjdaTSAxMC42NjM1IDYuNDk5NzFDIDEwLjcxNDIgNS4zMzg0NCAxMC4zNjA4IDQuMTk1NjIgOS42NjM3IDMuMjY2ODNDIDkuMzIxNjcgMi44NzkzNiA4Ljg5NjcyIDIuNTc0NDIgOC40MjA2MSAyLjM3NDc5QyA3Ljk0NDUxIDIuMTc1MTcgNy40Mjk1NiAyLjA4NjAxIDYuOTE0MjUgMi4xMTRDIDYuMzc2MzYgMi4xMDczOSA1Ljg0Mjk4IDIuMjEzMTcgNS4zNDgxIDIuNDI0NTlDIDQuODUzMjIgMi42MzYwMSA0LjQwNzY2IDIuOTQ4NDUgNC4wMzk4MyAzLjM0MjAxQyAzLjIxNzE2IDQuMjE0NiAyLjY5OTU5IDUuMzMxNjIgMi41NjUxMiA2LjUyNDc4TCAxMC42NjM1IDYuNDk5NzFaIi8+DQo8L2RlZnM+DQo8L3N2Zz4NCg==" });
            styles.Add(new ElementStyle("APIFY"){ Background = "#07059B", Color = "#ffffff", Shape = Shape.RoundedBox ,Icon= "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTI1IiBoZWlnaHQ9Ijc4IiB2aWV3Qm94PSIwIDAgMTI1IDc4IiB2ZXJzaW9uPSIxLjEiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiPg0KPHRpdGxlPm5ldGNvcmU8L3RpdGxlPg0KPGRlc2M+Q3JlYXRlZCB1c2luZyBGaWdtYTwvZGVzYz4NCjxnIGlkPSJDYW52YXMiIHRyYW5zZm9ybT0idHJhbnNsYXRlKC0zNjE4IC00MTQpIj4NCjxnIGlkPSJuZXRjb3JlIj4NCjxnIGlkPSJWZWN0b3IiPg0KPHVzZSB4bGluazpocmVmPSIjcGF0aDBfZmlsbCIgdHJhbnNmb3JtPSJ0cmFuc2xhdGUoMzYxOCA0NTMuMTcpIiBmaWxsPSIjNjIxRUU1Ii8+DQo8L2c+DQo8ZyBpZD0iVmVjdG9yIj4NCjx1c2UgeGxpbms6aHJlZj0iI3BhdGgxX2ZpbGwiIHRyYW5zZm9ybT0idHJhbnNsYXRlKDM2MzQuNSA0MTQpIiBmaWxsPSIjNjIxRUU1Ii8+DQo8L2c+DQo8ZyBpZD0iVmVjdG9yIj4NCjx1c2UgeGxpbms6aHJlZj0iI3BhdGgyX2ZpbGwiIHRyYW5zZm9ybT0idHJhbnNsYXRlKDM2ODMuMDEgNDE0KSIgZmlsbD0iIzYyMUVFNSIvPg0KPC9nPg0KPGcgaWQ9IlZlY3RvciI+DQo8dXNlIHhsaW5rOmhyZWY9IiNwYXRoM19maWxsIiB0cmFuc2Zvcm09InRyYW5zbGF0ZSgzNzExLjMzIDQxNCkiIGZpbGw9IiM2MjFFRTUiLz4NCjwvZz4NCjxnIGlkPSJWZWN0b3IiPg0KPHVzZSB4bGluazpocmVmPSIjcGF0aDRfZmlsbCIgdHJhbnNmb3JtPSJ0cmFuc2xhdGUoMzY1Mi42MyA0NzAuMzA1KSIgZmlsbD0iIzYyMUVFNSIvPg0KPC9nPg0KPGcgaWQ9IlZlY3RvciI+DQo8dXNlIHhsaW5rOmhyZWY9IiNwYXRoNV9maWxsIiB0cmFuc2Zvcm09InRyYW5zbGF0ZSgzNjcxLjIzIDQ3Ni4xMTcpIiBmaWxsPSIjNjIxRUU1Ii8+DQo8L2c+DQo8ZyBpZD0iVmVjdG9yIj4NCjx1c2UgeGxpbms6aHJlZj0iI3BhdGg2X2ZpbGwiIHRyYW5zZm9ybT0idHJhbnNsYXRlKDM2ODkuODEgNDc2LjM0OCkiIGZpbGw9IiM2MjFFRTUiLz4NCjwvZz4NCjxnIGlkPSJWZWN0b3IiPg0KPHVzZSB4bGluazpocmVmPSIjcGF0aDdfZmlsbCIgdHJhbnNmb3JtPSJ0cmFuc2xhdGUoMzY5OS4xNCA0NzYuMjE4KSIgZmlsbD0iIzYyMUVFNSIvPg0KPC9nPg0KPC9nPg0KPC9nPg0KPGRlZnM+DQo8cGF0aCBpZD0icGF0aDBfZmlsbCIgZD0iTSAzLjUyNDU5IDcuMTQyNjhDIDMuMDU4MDMgNy4xNTAwNSAyLjU5NSA3LjA2MDMyIDIuMTY0NzggNi44NzkxNUMgMS43MzQ1NiA2LjY5Nzk4IDEuMzQ2NDkgNi40MjkzMSAxLjAyNTEgNi4wOTAxMkMgMC42OTQ4ODYgNS43NjMwMiAwLjQzMzkxOSA1LjM3MjQ1IDAuMjU3ODI5IDQuOTQxODNDIDAuMDgxNzM4NSA0LjUxMTIgLTAuMDA1ODUyNjggNC4wNDkzNyAwLjAwMDMwMjc0IDMuNTg0QyAtMC4wMDMwMzEzMSAzLjExOTAyIDAuMDg1ODQ5OSAyLjY1ODAyIDAuMjYxNzc5IDIuMjI3NzlDIDAuNDM3NzA3IDEuNzk3NTYgMC42OTcxNzUgMS40MDY2OCAxLjAyNTEgMS4wNzc4OEMgMS4zNDMzNCAwLjczMjkxNyAxLjczMDI0IDAuNDU4NzUzIDIuMTYwNzkgMC4yNzMxMjVDIDIuNTkxMzUgMC4wODc0OTc4IDMuMDU1OTYgLTAuMDA1NDYxMzkgMy41MjQ1OSAwLjAwMDI1MjA4NEMgMy45OTI1NSAtMC4wMDA4OTU4MjMgNC40NTU3NyAwLjA5NDEyOCA0Ljg4NTY3IDAuMjc5NDczQyA1LjMxNTU3IDAuNDY0ODE5IDUuNzAzMDEgMC43MzY1NTIgNi4wMjQwOSAxLjA3Nzg4QyA2LjM1NzY3IDEuNDAzNzQgNi42MjI1IDEuNzkzNTEgNi44MDI4NCAyLjIyNDAzQyA2Ljk4MzE4IDIuNjU0NTUgNy4wNzUzNiAzLjExNzA1IDcuMDczODggMy41ODRDIDcuMDc4MjQgNC4wNTEzOCA2Ljk4NzM5IDQuNTE0NzIgNi44MDY4NyA0Ljk0NTY1QyA2LjYyNjM2IDUuMzc2NTggNi4zNTk5OSA1Ljc2NjAzIDYuMDI0MDkgNi4wOTAxMkMgNS42OTk5NyA2LjQyNTczIDUuMzExMzkgNi42OTIwMiA0Ljg4MTggNi44NzI5MkMgNC40NTIyMSA3LjA1MzgzIDMuOTkwNTMgNy4xNDU2IDMuNTI0NTkgNy4xNDI2OFoiLz4NCjxwYXRoIGlkPSJwYXRoMV9maWxsIiBkPSJNIDM2LjY5MjYgNDUuNjg2NUwgMzAuMTQzOSA0NS42ODY1TCA2LjcyMzY1IDkuMjQ3NTdDIDYuMTQwMjYgOC4zNDk3NiA1LjY1NDI3IDcuMzkxOTkgNS4yNzM5NCA2LjM5MDZMIDUuMDczOTggNi4zOTA2QyA1LjI5NjU1IDguNDcxMSA1LjM4MDA2IDEwLjU2NDIgNS4zMjM5MyAxMi42NTU5TCA1LjMyMzkzIDQ1LjY4NjVMIDMuODEzOTNlLTA3IDQ1LjY4NjVMIDMuODEzOTNlLTA3IDEuMTQ3MjFlLTA2TCA2LjkyMzYxIDEuMTQ3MjFlLTA2TCAyOS43MTkgMzUuNzg3M0MgMzAuNjY4OCAzNy4yNzQzIDMxLjI4NTQgMzguMjkzNSAzMS41Njg3IDM4Ljg0NDhMIDMxLjY5MzYgMzguODQ0OEMgMzEuNDI5OSAzNi42MTYzIDMxLjMyMTMgMzQuMzcyMSAzMS4zNjg3IDMyLjEyODRMIDMxLjM2ODcgMS4xNDcyMWUtMDZMIDM2LjY5MjYgMS4xNDcyMWUtMDZMIDM2LjY5MjYgNDUuNjg2NVoiLz4NCjxwYXRoIGlkPSJwYXRoMl9maWxsIiBkPSJNIDI0LjIyMDEgNDUuNjg2NUwgMCA0NS42ODY1TCAwIDEuMTQ3MjFlLTA2TCAyMy4wOTU0IDEuMTQ3MjFlLTA2TCAyMy4wOTU0IDQuODM2ODFMIDUuNDQ4OSA0LjgzNjgxTCA1LjQ0ODkgMjAuMDIzOUwgMjEuODk1NiAyMC4wMjM5TCAyMS44OTU2IDI0LjgzNTZMIDUuNDQ4OSAyNC44MzU2TCA1LjQ0ODkgNDAuNjc0M0wgMjQuMjQ1MSA0MC42NzQzTCAyNC4yMjAxIDQ1LjY4NjVaIi8+DQo8cGF0aCBpZD0icGF0aDNfZmlsbCIgZD0iTSAzMS42Njg2IDQuODM2ODFMIDE4LjQyMTMgNC44MzY4MUwgMTguNDIxMyA0NS42ODY1TCAxMy4wOTc0IDQ1LjY4NjVMIDEzLjA5NzQgNC44MzY4MUwgNC41NzY3MmUtMDYgNC44MzY4MUwgNC41NzY3MmUtMDYgMS4xNDcyMWUtMDZMIDMxLjY2ODYgMS4xNDcyMWUtMDZMIDMxLjY2ODYgNC44MzY4MVoiLz4NCjxwYXRoIGlkPSJwYXRoNF9maWxsIiBkPSJNIDE1LjY4MzIgMjAuNDU3MUMgMTMuODc4NiAyMS4zMzM0IDExLjg4ODEgMjEuNzU0OSA5Ljg4NDMyIDIxLjY4NTFDIDguNTUyOCAyMS43NDUzIDcuMjIzODUgMjEuNTE5MiA1Ljk4NjYxIDIxLjAyMjFDIDQuNzQ5MzcgMjAuNTI1MSAzLjYzMjQ1IDE5Ljc2ODUgMi43MTA3NyAxOC44MDMxQyAwLjg3NDIwNyAxNi43NDE4IC0wLjA5MzQ2MjggMTQuMDQ2NyAwLjAxMTMwOTYgMTEuMjg0N0MgLTAuMDU1OTIyMyA5Ljc4OTI3IDAuMTc3MTQyIDguMjk1NDkgMC42OTY2NzMgNi44OTIwNkMgMS4yMTYyIDUuNDg4NjIgMi4wMTE2MSA0LjIwNDEzIDMuMDM1NyAzLjExNDc5QyA0LjAzMzcgMi4wODk2OSA1LjIzMzUzIDEuMjg0MDMgNi41NTg4OSAwLjc0OTAyN0MgNy44ODQyNSAwLjIxNDAyNyA5LjMwNjE0IC0wLjAzODU5OTIgMTAuNzM0MiAwLjAwNzIxMjg0QyAxMi40NDMxIC0wLjA1MTM0NDEgMTQuMTQ1NiAwLjI0NzM2NiAxNS43MzMxIDAuODg0MzVMIDE1LjczMzEgMy4zOTA0N0MgMTQuMjA0NiAyLjU0MjAyIDEyLjQ4MTEgMi4xMDk5OCAxMC43MzQyIDIuMTM3NDFDIDkuNjMyODMgMi4xMDYzMSA4LjUzNzggMi4zMTQxNCA3LjUyNDAxIDIuNzQ2NjhDIDYuNTEwMjIgMy4xNzkyMiA1LjYwMTYxIDMuODI2MjYgNC44NjAzNCA0LjY0MzUzQyAzLjMwMTUxIDYuNDI0MTEgMi40OTQ3NSA4Ljc0MjkyIDIuNjEwNzggMTEuMTA5M0MgMi40OTQwNyAxMy4zNTEgMy4yNDYzOSAxNS41NTEgNC43MTAzNiAxNy4yNDkzQyA1LjQxMjU4IDE4LjAwNjQgNi4yNjk4NCAxOC42MDIyIDcuMjIzMzIgMTguOTk1OUMgOC4xNzY4MSAxOS4zODk1IDkuMjAzOTUgMTkuNTcxNyAxMC4yMzQzIDE5LjUyOTlDIDEyLjE1MzQgMTkuNTgwMiAxNC4wNDg3IDE5LjA5NDMgMTUuNzA4MiAxOC4xMjY0TCAxNS42ODMyIDIwLjQ1NzFaIi8+DQo8cGF0aCBpZD0icGF0aDVfZmlsbCIgZD0iTSA3LjMxMjIzIDE1Ljg3MzNDIDYuMzE3MjYgMTUuOTA1NiA1LjMyNjk5IDE1LjcyMzEgNC40MDg0NSAxNS4zMzgzQyAzLjQ4OTkyIDE0Ljk1MzUgMi42NjQ1OCAxNC4zNzUzIDEuOTg4MyAxMy42NDI4QyAwLjYyMTUyOSAxMi4xMTM1IC0wLjA4ODA3MDUgMTAuMTA1MSAwLjAxMzcwNCA4LjA1NDE4QyAtMC4wNDg3MTMzIDYuOTc5NjggMC4xMDI1NTEgNS45MDM0NSAwLjQ1ODcwNCA0Ljg4ODFDIDAuODE0ODU2IDMuODcyNzQgMS4zNjg3OSAyLjkzODUxIDIuMDg4MjkgMi4xMzk3NEMgMi44MjI1OCAxLjQyMTIxIDMuNjk2MTIgMC44NjE0MTIgNC42NTQ4NyAwLjQ5NTAwM0MgNS42MTM2MiAwLjEyODU5MyA2LjYzNzE3IC0wLjAzNjYzMTggNy42NjIxNyAwLjAwOTU0MTk5QyA4LjYzMTAzIC0wLjA0MjE4NzkgOS41OTk1OSAwLjExNjkzNCAxMC41MDEzIDAuNDc1OTg4QyAxMS40MDMxIDAuODM1MDQyIDEyLjIxNjYgMS4zODU1MiAxMi44ODYxIDIuMDg5NjFDIDE0LjIxNDYgMy42OTQ4NSAxNC44ODQ3IDUuNzQ2NDMgMTQuNzYwNyA3LjgyODYyQyAxNC44NjU2IDkuOTQxOCAxNC4xMzg2IDEyLjAxMTcgMTIuNzM2MSAxMy41OTI3QyAxMi4wNDcxIDE0LjMzOTQgMTEuMjA2NSAxNC45MjkzIDEwLjI3MDggMTUuMzIyN0MgOS4zMzUyMSAxNS43MTYxIDguMzI2MzQgMTUuOTAzOCA3LjMxMjIzIDE1Ljg3MzNMIDcuMzEyMjMgMTUuODczM1pNIDcuNDg3MiAyLjE2NDc5QyA2Ljc5OTY1IDIuMTM1NzggNi4xMTQ0NCAyLjI2MjY4IDUuNDgyNTkgMi41MzYwNUMgNC44NTA3NSAyLjgwOTQyIDQuMjg4NTcgMy4yMjIyIDMuODM3OTMgMy43NDM2NUMgMi44NzYxNiA0Ljk4NTM0IDIuMzk2NTYgNi41MzQ4MiAyLjQ4ODIxIDguMTA0M0MgMi40MDA5MSA5LjYyNzEgMi44ODIwNiAxMS4xMjggMy44Mzc5MyAxMi4zMTQ2QyA0LjI5MzI1IDEyLjgyODcgNC44NTY5IDEzLjIzNDggNS40ODgwNSAxMy41MDM1QyA2LjExOTIxIDEzLjc3MjIgNi44MDIwOCAxMy44OTY5IDcuNDg3MiAxMy44Njg0QyA4LjE1Nzk0IDEzLjkwODEgOC44Mjg4NCAxMy43OTMyIDkuNDQ4NDIgMTMuNTMyNUMgMTAuMDY4IDEzLjI3MTkgMTAuNjE5OCAxMi44NzI0IDExLjA2MTUgMTIuMzY0N0MgMTEuOTcwMiAxMS4xMTk3IDEyLjQxMjcgOS41OTM2MSAxMi4zMTEyIDguMDU0MThDIDEyLjQxMDEgNi41MDcyNSAxMS45NjgxIDQuOTc0MTUgMTEuMDYxNSAzLjcxODU5QyAxMC42MjU5IDMuMjAwMzEgMTAuMDc2NSAyLjc5MDI4IDkuNDU2MzkgMi41MjA2OUMgOC44MzYyNCAyLjI1MTEgOC4xNjIyMSAyLjEyOTI4IDcuNDg3MiAyLjE2NDc5TCA3LjQ4NzIgMi4xNjQ3OVoiLz4NCjxwYXRoIGlkPSJwYXRoNl9maWxsIiBkPSJNIDcuODQ4NDIgMi42ODU1MkMgNy4zMDg1NiAyLjMzMDkgNi42Njc1OCAyLjE2MzYzIDYuMDIzNzkgMi4yMDkzNUMgNS41MjAyNyAyLjIyNzcyIDUuMDI4OTYgMi4zNjk4OSA0LjU5MzExIDIuNjIzMzZDIDQuMTU3MjYgMi44NzY4MiAzLjc5MDIzIDMuMjMzODEgMy41MjQyOSAzLjY2MjlDIDIuNzgwMzYgNC44NDM4NCAyLjQyMjE0IDYuMjI3OTQgMi40OTk1IDcuNjIyNTZMIDIuNDk5NSAxNS4yOTEzTCAzLjA1MTE0ZS0wNiAxNS4yOTEzTCAzLjA1MTE0ZS0wNiAwLjI1NDU3OUwgMi40OTk1IDAuMjU0NTc5TCAyLjQ5OTUgMy4zODcyM0MgMi43NzQyOCAyLjQxNzkzIDMuMzIwODEgMS41NDgxIDQuMDc0MTggMC44ODExMDhDIDQuNzI2MDQgMC4zMTM4ODYgNS41NjA2IDAuMDAyMzI4OTEgNi40MjM3MSAwLjAwMzk3MTE5QyA2LjkwNjg4IC0wLjAxNzgwMDEgNy4zODk5IDAuMDUwMTY2NyA3Ljg0ODQyIDAuMjA0NDU2TCA3Ljg0ODQyIDIuNjg1NTJaIi8+DQo8cGF0aCBpZD0icGF0aDdfZmlsbCIgZD0iTSAxMy4xNjMgOC41Mjk2N0wgMi41OTAxMiA4LjUyOTY3QyAyLjUxOTE4IDkuOTMwNjUgMi45NzM5NCAxMS4zMDc1IDMuODY0ODYgMTIuMzg5MUMgNC4zMjgxNyAxMi44NTI0IDQuODgzOCAxMy4yMTI0IDUuNDk1NDIgMTMuNDQ1NkMgNi4xMDcwNCAxMy42Nzg4IDYuNzYwODcgMTMuNzggNy40MTQxNCAxMy43NDI0QyA5LjEwODU0IDEzLjcyNTUgMTAuNzQ3MyAxMy4xMzM2IDEyLjA2MzIgMTIuMDYzM0wgMTIuMDYzMiAxNC4zMTg4QyAxMC41MTc4IDE1LjM0MDEgOC42ODc4MSAxNS44NDA1IDYuODM5MjYgMTUuNzQ3M0MgNS45MDY1NSAxNS43OTQ1IDQuOTc0OTUgMTUuNjM0OCA0LjExMDg2IDE1LjI3OTZDIDMuMjQ2NzcgMTQuOTI0MyAyLjQ3MTQ4IDE0LjM4MjQgMS44NDAyNyAxMy42OTIzQyAwLjUzOTA0NCAxMi4wNjQ5IC0wLjExMTQgMTAuMDEwMSAwLjAxNTYzNSA3LjkyODE5QyAtMC4wNzQyMDEgNS44NDExNyAwLjYzMTgyNCAzLjc5ODE0IDEuOTkwMjQgMi4yMTQyNUMgMi42MDg0NSAxLjQ5ODIzIDMuMzc3NjYgMC45Mjg5MjIgNC4yNDIyMSAwLjU0NzUwOUMgNS4xMDY3NyAwLjE2NjA5NiA2LjA0NTA3IC0wLjAxNzg5MjYgNi45ODkyMyAwLjAwODg2NTM3QyA3Ljg0MTc2IC0wLjAzOTAyNTQgOC42OTQwMSAwLjEwNzQ4MyA5LjQ4MTkzIDAuNDM3Mzc3QyAxMC4yNjk5IDAuNzY3MjcgMTAuOTcyOSAxLjI3MTk3IDExLjUzODMgMS45MTM1MkMgMTIuNjg5MiAzLjQzNTE0IDEzLjI1NjkgNS4zMjA1MyAxMy4xMzggNy4yMjY0OUwgMTMuMTYzIDguNTI5NjdaTSAxMC42NjM1IDYuNDk5NzFDIDEwLjcxNDIgNS4zMzg0NCAxMC4zNjA4IDQuMTk1NjIgOS42NjM3IDMuMjY2ODNDIDkuMzIxNjcgMi44NzkzNiA4Ljg5NjcyIDIuNTc0NDIgOC40MjA2MSAyLjM3NDc5QyA3Ljk0NDUxIDIuMTc1MTcgNy40Mjk1NiAyLjA4NjAxIDYuOTE0MjUgMi4xMTRDIDYuMzc2MzYgMi4xMDczOSA1Ljg0Mjk4IDIuMjEzMTcgNS4zNDgxIDIuNDI0NTlDIDQuODUzMjIgMi42MzYwMSA0LjQwNzY2IDIuOTQ4NDUgNC4wMzk4MyAzLjM0MjAxQyAzLjIxNzE2IDQuMjE0NiAyLjY5OTU5IDUuMzMxNjIgMi41NjUxMiA2LjUyNDc4TCAxMC42NjM1IDYuNDk5NzFaIi8+DQo8L2RlZnM+DQo8L3N2Zz4NCg==" });
            styles.Add(new ElementStyle("DB"){ Background = "#6904FA", Color = "#ffffff", Shape = Shape.Cylinder });
            styles.Add(new ElementStyle("Landing"){ Background = "#07059B", Color = "#ffffff", Shape = Shape.WebBrowser, Icon= "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTI1IiBoZWlnaHQ9Ijc4IiB2aWV3Qm94PSIwIDAgMTI1IDc4IiB2ZXJzaW9uPSIxLjEiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiPg0KPHRpdGxlPm5ldGNvcmU8L3RpdGxlPg0KPGRlc2M+Q3JlYXRlZCB1c2luZyBGaWdtYTwvZGVzYz4NCjxnIGlkPSJDYW52YXMiIHRyYW5zZm9ybT0idHJhbnNsYXRlKC0zNjE4IC00MTQpIj4NCjxnIGlkPSJuZXRjb3JlIj4NCjxnIGlkPSJWZWN0b3IiPg0KPHVzZSB4bGluazpocmVmPSIjcGF0aDBfZmlsbCIgdHJhbnNmb3JtPSJ0cmFuc2xhdGUoMzYxOCA0NTMuMTcpIiBmaWxsPSIjNjIxRUU1Ii8+DQo8L2c+DQo8ZyBpZD0iVmVjdG9yIj4NCjx1c2UgeGxpbms6aHJlZj0iI3BhdGgxX2ZpbGwiIHRyYW5zZm9ybT0idHJhbnNsYXRlKDM2MzQuNSA0MTQpIiBmaWxsPSIjNjIxRUU1Ii8+DQo8L2c+DQo8ZyBpZD0iVmVjdG9yIj4NCjx1c2UgeGxpbms6aHJlZj0iI3BhdGgyX2ZpbGwiIHRyYW5zZm9ybT0idHJhbnNsYXRlKDM2ODMuMDEgNDE0KSIgZmlsbD0iIzYyMUVFNSIvPg0KPC9nPg0KPGcgaWQ9IlZlY3RvciI+DQo8dXNlIHhsaW5rOmhyZWY9IiNwYXRoM19maWxsIiB0cmFuc2Zvcm09InRyYW5zbGF0ZSgzNzExLjMzIDQxNCkiIGZpbGw9IiM2MjFFRTUiLz4NCjwvZz4NCjxnIGlkPSJWZWN0b3IiPg0KPHVzZSB4bGluazpocmVmPSIjcGF0aDRfZmlsbCIgdHJhbnNmb3JtPSJ0cmFuc2xhdGUoMzY1Mi42MyA0NzAuMzA1KSIgZmlsbD0iIzYyMUVFNSIvPg0KPC9nPg0KPGcgaWQ9IlZlY3RvciI+DQo8dXNlIHhsaW5rOmhyZWY9IiNwYXRoNV9maWxsIiB0cmFuc2Zvcm09InRyYW5zbGF0ZSgzNjcxLjIzIDQ3Ni4xMTcpIiBmaWxsPSIjNjIxRUU1Ii8+DQo8L2c+DQo8ZyBpZD0iVmVjdG9yIj4NCjx1c2UgeGxpbms6aHJlZj0iI3BhdGg2X2ZpbGwiIHRyYW5zZm9ybT0idHJhbnNsYXRlKDM2ODkuODEgNDc2LjM0OCkiIGZpbGw9IiM2MjFFRTUiLz4NCjwvZz4NCjxnIGlkPSJWZWN0b3IiPg0KPHVzZSB4bGluazpocmVmPSIjcGF0aDdfZmlsbCIgdHJhbnNmb3JtPSJ0cmFuc2xhdGUoMzY5OS4xNCA0NzYuMjE4KSIgZmlsbD0iIzYyMUVFNSIvPg0KPC9nPg0KPC9nPg0KPC9nPg0KPGRlZnM+DQo8cGF0aCBpZD0icGF0aDBfZmlsbCIgZD0iTSAzLjUyNDU5IDcuMTQyNjhDIDMuMDU4MDMgNy4xNTAwNSAyLjU5NSA3LjA2MDMyIDIuMTY0NzggNi44NzkxNUMgMS43MzQ1NiA2LjY5Nzk4IDEuMzQ2NDkgNi40MjkzMSAxLjAyNTEgNi4wOTAxMkMgMC42OTQ4ODYgNS43NjMwMiAwLjQzMzkxOSA1LjM3MjQ1IDAuMjU3ODI5IDQuOTQxODNDIDAuMDgxNzM4NSA0LjUxMTIgLTAuMDA1ODUyNjggNC4wNDkzNyAwLjAwMDMwMjc0IDMuNTg0QyAtMC4wMDMwMzEzMSAzLjExOTAyIDAuMDg1ODQ5OSAyLjY1ODAyIDAuMjYxNzc5IDIuMjI3NzlDIDAuNDM3NzA3IDEuNzk3NTYgMC42OTcxNzUgMS40MDY2OCAxLjAyNTEgMS4wNzc4OEMgMS4zNDMzNCAwLjczMjkxNyAxLjczMDI0IDAuNDU4NzUzIDIuMTYwNzkgMC4yNzMxMjVDIDIuNTkxMzUgMC4wODc0OTc4IDMuMDU1OTYgLTAuMDA1NDYxMzkgMy41MjQ1OSAwLjAwMDI1MjA4NEMgMy45OTI1NSAtMC4wMDA4OTU4MjMgNC40NTU3NyAwLjA5NDEyOCA0Ljg4NTY3IDAuMjc5NDczQyA1LjMxNTU3IDAuNDY0ODE5IDUuNzAzMDEgMC43MzY1NTIgNi4wMjQwOSAxLjA3Nzg4QyA2LjM1NzY3IDEuNDAzNzQgNi42MjI1IDEuNzkzNTEgNi44MDI4NCAyLjIyNDAzQyA2Ljk4MzE4IDIuNjU0NTUgNy4wNzUzNiAzLjExNzA1IDcuMDczODggMy41ODRDIDcuMDc4MjQgNC4wNTEzOCA2Ljk4NzM5IDQuNTE0NzIgNi44MDY4NyA0Ljk0NTY1QyA2LjYyNjM2IDUuMzc2NTggNi4zNTk5OSA1Ljc2NjAzIDYuMDI0MDkgNi4wOTAxMkMgNS42OTk5NyA2LjQyNTczIDUuMzExMzkgNi42OTIwMiA0Ljg4MTggNi44NzI5MkMgNC40NTIyMSA3LjA1MzgzIDMuOTkwNTMgNy4xNDU2IDMuNTI0NTkgNy4xNDI2OFoiLz4NCjxwYXRoIGlkPSJwYXRoMV9maWxsIiBkPSJNIDM2LjY5MjYgNDUuNjg2NUwgMzAuMTQzOSA0NS42ODY1TCA2LjcyMzY1IDkuMjQ3NTdDIDYuMTQwMjYgOC4zNDk3NiA1LjY1NDI3IDcuMzkxOTkgNS4yNzM5NCA2LjM5MDZMIDUuMDczOTggNi4zOTA2QyA1LjI5NjU1IDguNDcxMSA1LjM4MDA2IDEwLjU2NDIgNS4zMjM5MyAxMi42NTU5TCA1LjMyMzkzIDQ1LjY4NjVMIDMuODEzOTNlLTA3IDQ1LjY4NjVMIDMuODEzOTNlLTA3IDEuMTQ3MjFlLTA2TCA2LjkyMzYxIDEuMTQ3MjFlLTA2TCAyOS43MTkgMzUuNzg3M0MgMzAuNjY4OCAzNy4yNzQzIDMxLjI4NTQgMzguMjkzNSAzMS41Njg3IDM4Ljg0NDhMIDMxLjY5MzYgMzguODQ0OEMgMzEuNDI5OSAzNi42MTYzIDMxLjMyMTMgMzQuMzcyMSAzMS4zNjg3IDMyLjEyODRMIDMxLjM2ODcgMS4xNDcyMWUtMDZMIDM2LjY5MjYgMS4xNDcyMWUtMDZMIDM2LjY5MjYgNDUuNjg2NVoiLz4NCjxwYXRoIGlkPSJwYXRoMl9maWxsIiBkPSJNIDI0LjIyMDEgNDUuNjg2NUwgMCA0NS42ODY1TCAwIDEuMTQ3MjFlLTA2TCAyMy4wOTU0IDEuMTQ3MjFlLTA2TCAyMy4wOTU0IDQuODM2ODFMIDUuNDQ4OSA0LjgzNjgxTCA1LjQ0ODkgMjAuMDIzOUwgMjEuODk1NiAyMC4wMjM5TCAyMS44OTU2IDI0LjgzNTZMIDUuNDQ4OSAyNC44MzU2TCA1LjQ0ODkgNDAuNjc0M0wgMjQuMjQ1MSA0MC42NzQzTCAyNC4yMjAxIDQ1LjY4NjVaIi8+DQo8cGF0aCBpZD0icGF0aDNfZmlsbCIgZD0iTSAzMS42Njg2IDQuODM2ODFMIDE4LjQyMTMgNC44MzY4MUwgMTguNDIxMyA0NS42ODY1TCAxMy4wOTc0IDQ1LjY4NjVMIDEzLjA5NzQgNC44MzY4MUwgNC41NzY3MmUtMDYgNC44MzY4MUwgNC41NzY3MmUtMDYgMS4xNDcyMWUtMDZMIDMxLjY2ODYgMS4xNDcyMWUtMDZMIDMxLjY2ODYgNC44MzY4MVoiLz4NCjxwYXRoIGlkPSJwYXRoNF9maWxsIiBkPSJNIDE1LjY4MzIgMjAuNDU3MUMgMTMuODc4NiAyMS4zMzM0IDExLjg4ODEgMjEuNzU0OSA5Ljg4NDMyIDIxLjY4NTFDIDguNTUyOCAyMS43NDUzIDcuMjIzODUgMjEuNTE5MiA1Ljk4NjYxIDIxLjAyMjFDIDQuNzQ5MzcgMjAuNTI1MSAzLjYzMjQ1IDE5Ljc2ODUgMi43MTA3NyAxOC44MDMxQyAwLjg3NDIwNyAxNi43NDE4IC0wLjA5MzQ2MjggMTQuMDQ2NyAwLjAxMTMwOTYgMTEuMjg0N0MgLTAuMDU1OTIyMyA5Ljc4OTI3IDAuMTc3MTQyIDguMjk1NDkgMC42OTY2NzMgNi44OTIwNkMgMS4yMTYyIDUuNDg4NjIgMi4wMTE2MSA0LjIwNDEzIDMuMDM1NyAzLjExNDc5QyA0LjAzMzcgMi4wODk2OSA1LjIzMzUzIDEuMjg0MDMgNi41NTg4OSAwLjc0OTAyN0MgNy44ODQyNSAwLjIxNDAyNyA5LjMwNjE0IC0wLjAzODU5OTIgMTAuNzM0MiAwLjAwNzIxMjg0QyAxMi40NDMxIC0wLjA1MTM0NDEgMTQuMTQ1NiAwLjI0NzM2NiAxNS43MzMxIDAuODg0MzVMIDE1LjczMzEgMy4zOTA0N0MgMTQuMjA0NiAyLjU0MjAyIDEyLjQ4MTEgMi4xMDk5OCAxMC43MzQyIDIuMTM3NDFDIDkuNjMyODMgMi4xMDYzMSA4LjUzNzggMi4zMTQxNCA3LjUyNDAxIDIuNzQ2NjhDIDYuNTEwMjIgMy4xNzkyMiA1LjYwMTYxIDMuODI2MjYgNC44NjAzNCA0LjY0MzUzQyAzLjMwMTUxIDYuNDI0MTEgMi40OTQ3NSA4Ljc0MjkyIDIuNjEwNzggMTEuMTA5M0MgMi40OTQwNyAxMy4zNTEgMy4yNDYzOSAxNS41NTEgNC43MTAzNiAxNy4yNDkzQyA1LjQxMjU4IDE4LjAwNjQgNi4yNjk4NCAxOC42MDIyIDcuMjIzMzIgMTguOTk1OUMgOC4xNzY4MSAxOS4zODk1IDkuMjAzOTUgMTkuNTcxNyAxMC4yMzQzIDE5LjUyOTlDIDEyLjE1MzQgMTkuNTgwMiAxNC4wNDg3IDE5LjA5NDMgMTUuNzA4MiAxOC4xMjY0TCAxNS42ODMyIDIwLjQ1NzFaIi8+DQo8cGF0aCBpZD0icGF0aDVfZmlsbCIgZD0iTSA3LjMxMjIzIDE1Ljg3MzNDIDYuMzE3MjYgMTUuOTA1NiA1LjMyNjk5IDE1LjcyMzEgNC40MDg0NSAxNS4zMzgzQyAzLjQ4OTkyIDE0Ljk1MzUgMi42NjQ1OCAxNC4zNzUzIDEuOTg4MyAxMy42NDI4QyAwLjYyMTUyOSAxMi4xMTM1IC0wLjA4ODA3MDUgMTAuMTA1MSAwLjAxMzcwNCA4LjA1NDE4QyAtMC4wNDg3MTMzIDYuOTc5NjggMC4xMDI1NTEgNS45MDM0NSAwLjQ1ODcwNCA0Ljg4ODFDIDAuODE0ODU2IDMuODcyNzQgMS4zNjg3OSAyLjkzODUxIDIuMDg4MjkgMi4xMzk3NEMgMi44MjI1OCAxLjQyMTIxIDMuNjk2MTIgMC44NjE0MTIgNC42NTQ4NyAwLjQ5NTAwM0MgNS42MTM2MiAwLjEyODU5MyA2LjYzNzE3IC0wLjAzNjYzMTggNy42NjIxNyAwLjAwOTU0MTk5QyA4LjYzMTAzIC0wLjA0MjE4NzkgOS41OTk1OSAwLjExNjkzNCAxMC41MDEzIDAuNDc1OTg4QyAxMS40MDMxIDAuODM1MDQyIDEyLjIxNjYgMS4zODU1MiAxMi44ODYxIDIuMDg5NjFDIDE0LjIxNDYgMy42OTQ4NSAxNC44ODQ3IDUuNzQ2NDMgMTQuNzYwNyA3LjgyODYyQyAxNC44NjU2IDkuOTQxOCAxNC4xMzg2IDEyLjAxMTcgMTIuNzM2MSAxMy41OTI3QyAxMi4wNDcxIDE0LjMzOTQgMTEuMjA2NSAxNC45MjkzIDEwLjI3MDggMTUuMzIyN0MgOS4zMzUyMSAxNS43MTYxIDguMzI2MzQgMTUuOTAzOCA3LjMxMjIzIDE1Ljg3MzNMIDcuMzEyMjMgMTUuODczM1pNIDcuNDg3MiAyLjE2NDc5QyA2Ljc5OTY1IDIuMTM1NzggNi4xMTQ0NCAyLjI2MjY4IDUuNDgyNTkgMi41MzYwNUMgNC44NTA3NSAyLjgwOTQyIDQuMjg4NTcgMy4yMjIyIDMuODM3OTMgMy43NDM2NUMgMi44NzYxNiA0Ljk4NTM0IDIuMzk2NTYgNi41MzQ4MiAyLjQ4ODIxIDguMTA0M0MgMi40MDA5MSA5LjYyNzEgMi44ODIwNiAxMS4xMjggMy44Mzc5MyAxMi4zMTQ2QyA0LjI5MzI1IDEyLjgyODcgNC44NTY5IDEzLjIzNDggNS40ODgwNSAxMy41MDM1QyA2LjExOTIxIDEzLjc3MjIgNi44MDIwOCAxMy44OTY5IDcuNDg3MiAxMy44Njg0QyA4LjE1Nzk0IDEzLjkwODEgOC44Mjg4NCAxMy43OTMyIDkuNDQ4NDIgMTMuNTMyNUMgMTAuMDY4IDEzLjI3MTkgMTAuNjE5OCAxMi44NzI0IDExLjA2MTUgMTIuMzY0N0MgMTEuOTcwMiAxMS4xMTk3IDEyLjQxMjcgOS41OTM2MSAxMi4zMTEyIDguMDU0MThDIDEyLjQxMDEgNi41MDcyNSAxMS45NjgxIDQuOTc0MTUgMTEuMDYxNSAzLjcxODU5QyAxMC42MjU5IDMuMjAwMzEgMTAuMDc2NSAyLjc5MDI4IDkuNDU2MzkgMi41MjA2OUMgOC44MzYyNCAyLjI1MTEgOC4xNjIyMSAyLjEyOTI4IDcuNDg3MiAyLjE2NDc5TCA3LjQ4NzIgMi4xNjQ3OVoiLz4NCjxwYXRoIGlkPSJwYXRoNl9maWxsIiBkPSJNIDcuODQ4NDIgMi42ODU1MkMgNy4zMDg1NiAyLjMzMDkgNi42Njc1OCAyLjE2MzYzIDYuMDIzNzkgMi4yMDkzNUMgNS41MjAyNyAyLjIyNzcyIDUuMDI4OTYgMi4zNjk4OSA0LjU5MzExIDIuNjIzMzZDIDQuMTU3MjYgMi44NzY4MiAzLjc5MDIzIDMuMjMzODEgMy41MjQyOSAzLjY2MjlDIDIuNzgwMzYgNC44NDM4NCAyLjQyMjE0IDYuMjI3OTQgMi40OTk1IDcuNjIyNTZMIDIuNDk5NSAxNS4yOTEzTCAzLjA1MTE0ZS0wNiAxNS4yOTEzTCAzLjA1MTE0ZS0wNiAwLjI1NDU3OUwgMi40OTk1IDAuMjU0NTc5TCAyLjQ5OTUgMy4zODcyM0MgMi43NzQyOCAyLjQxNzkzIDMuMzIwODEgMS41NDgxIDQuMDc0MTggMC44ODExMDhDIDQuNzI2MDQgMC4zMTM4ODYgNS41NjA2IDAuMDAyMzI4OTEgNi40MjM3MSAwLjAwMzk3MTE5QyA2LjkwNjg4IC0wLjAxNzgwMDEgNy4zODk5IDAuMDUwMTY2NyA3Ljg0ODQyIDAuMjA0NDU2TCA3Ljg0ODQyIDIuNjg1NTJaIi8+DQo8cGF0aCBpZD0icGF0aDdfZmlsbCIgZD0iTSAxMy4xNjMgOC41Mjk2N0wgMi41OTAxMiA4LjUyOTY3QyAyLjUxOTE4IDkuOTMwNjUgMi45NzM5NCAxMS4zMDc1IDMuODY0ODYgMTIuMzg5MUMgNC4zMjgxNyAxMi44NTI0IDQuODgzOCAxMy4yMTI0IDUuNDk1NDIgMTMuNDQ1NkMgNi4xMDcwNCAxMy42Nzg4IDYuNzYwODcgMTMuNzggNy40MTQxNCAxMy43NDI0QyA5LjEwODU0IDEzLjcyNTUgMTAuNzQ3MyAxMy4xMzM2IDEyLjA2MzIgMTIuMDYzM0wgMTIuMDYzMiAxNC4zMTg4QyAxMC41MTc4IDE1LjM0MDEgOC42ODc4MSAxNS44NDA1IDYuODM5MjYgMTUuNzQ3M0MgNS45MDY1NSAxNS43OTQ1IDQuOTc0OTUgMTUuNjM0OCA0LjExMDg2IDE1LjI3OTZDIDMuMjQ2NzcgMTQuOTI0MyAyLjQ3MTQ4IDE0LjM4MjQgMS44NDAyNyAxMy42OTIzQyAwLjUzOTA0NCAxMi4wNjQ5IC0wLjExMTQgMTAuMDEwMSAwLjAxNTYzNSA3LjkyODE5QyAtMC4wNzQyMDEgNS44NDExNyAwLjYzMTgyNCAzLjc5ODE0IDEuOTkwMjQgMi4yMTQyNUMgMi42MDg0NSAxLjQ5ODIzIDMuMzc3NjYgMC45Mjg5MjIgNC4yNDIyMSAwLjU0NzUwOUMgNS4xMDY3NyAwLjE2NjA5NiA2LjA0NTA3IC0wLjAxNzg5MjYgNi45ODkyMyAwLjAwODg2NTM3QyA3Ljg0MTc2IC0wLjAzOTAyNTQgOC42OTQwMSAwLjEwNzQ4MyA5LjQ4MTkzIDAuNDM3Mzc3QyAxMC4yNjk5IDAuNzY3MjcgMTAuOTcyOSAxLjI3MTk3IDExLjUzODMgMS45MTM1MkMgMTIuNjg5MiAzLjQzNTE0IDEzLjI1NjkgNS4zMjA1MyAxMy4xMzggNy4yMjY0OUwgMTMuMTYzIDguNTI5NjdaTSAxMC42NjM1IDYuNDk5NzFDIDEwLjcxNDIgNS4zMzg0NCAxMC4zNjA4IDQuMTk1NjIgOS42NjM3IDMuMjY2ODNDIDkuMzIxNjcgMi44NzkzNiA4Ljg5NjcyIDIuNTc0NDIgOC40MjA2MSAyLjM3NDc5QyA3Ljk0NDUxIDIuMTc1MTcgNy40Mjk1NiAyLjA4NjAxIDYuOTE0MjUgMi4xMTRDIDYuMzc2MzYgMi4xMDczOSA1Ljg0Mjk4IDIuMjEzMTcgNS4zNDgxIDIuNDI0NTlDIDQuODUzMjIgMi42MzYwMSA0LjQwNzY2IDIuOTQ4NDUgNC4wMzk4MyAzLjM0MjAxQyAzLjIxNzE2IDQuMjE0NiAyLjY5OTU5IDUuMzMxNjIgMi41NjUxMiA2LjUyNDc4TCAxMC42NjM1IDYuNDk5NzFaIi8+DQo8L2RlZnM+DQo8L3N2Zz4NCg==" });


            //////////////////
            
           
            ContainerView containerFYView = viewSet.CreateContainerView(appSystem, "ContenedorFY", "Diagrama de contenedores - FoodYeah");
           
            contextFYView.PaperSize = PaperSize.A4_Landscape;
            
            containerFYView.AddAllElements();

            // 3. Diagrama de Componentes
            
         

           
           
           


            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}