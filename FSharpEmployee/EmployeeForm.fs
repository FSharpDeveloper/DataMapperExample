namespace FSharpEmployee

open System
open System.Collections.Generic
open System.Windows.Forms
open System.Drawing
open System.Data
open DataMapper

type public EmployeeForm() as form =    
    inherit Form() 

    let lblId = new Label()
    let txtId = new TextBox()
    let txtFirstName = new TextBox()
    let lblFirstName = new Label()
    let txtLastName = new TextBox()
    let lblLastName = new Label()
    let txtAddresse = new TextBox()
    let lblAddresse = new Label()
    let txtDate = new TextBox()
    let lblDate = new Label()
    let btnAjouter = new Button()
    let btnModifier = new Button()
    let btnSupprimer = new Button()
    let btnPrecedant = new Button()
    let btnSuivant = new Button()
    let btnDernier = new Button()
    let btnPremier = new Button()
    let btnDel = new Button()
    let btnAdd = new Button()
    let lblNotification = new Label()
    let label1 = new Label()
    let txtChercher = new TextBox()
    
    let mapper = new EmployeeDataMapper()
    
    do form.InitializeComponents 

    member this.InitializeComponents = 
        lblId.AutoSize <- true
        lblId.Location <- Point(59, 42)
        lblId.Name <- "lblId"
        lblId.Size <- Size(19, 13)
        lblId.TabIndex <- 0
        lblId.Text <- "Id:"

        txtId.Location <- Point(123, 42)
        txtId.Name <- "txtId"
        txtId.Size <- Size(100, 20)
        txtId.TabIndex <- 1
        // 
        // lblFirstName
        // 
        lblFirstName.AutoSize <- true
        lblFirstName.Location <- Point(59, 68)
        lblFirstName.Name <- "lblFirstName"
        lblFirstName.Size <- Size(60, 13)
        lblFirstName.TabIndex <- 2
        lblFirstName.Text <- "First Name:"
        // 
        // txtFirstName
        // 
        txtFirstName.Location <- Point(123, 68)
        txtFirstName.Name <- "txtFirstName"
        txtFirstName.Size <- Size(100, 20)
        txtFirstName.TabIndex <- 3
        // 
        // lblLastName
        // 
        lblLastName.AutoSize <- true
        lblLastName.Location <- Point(59, 94)
        lblLastName.Name <- "lblLastName"
        lblLastName.Size <- Size(61, 13)
        lblLastName.TabIndex <- 4
        lblLastName.Text <- "Last Name:"
        // 
        // txtLastName
        // 
        txtLastName.Location <- Point(123, 94)
        txtLastName.Name <- "txtLastName"
        txtLastName.Size <- Size(100, 20)
        txtLastName.TabIndex <- 5
        // 
        // lblAddresse
        // 
        lblAddresse.AutoSize <- true
        lblAddresse.Location <- Point(59, 120)
        lblAddresse.Name <- "lblAddresse"
        lblAddresse.Size <- Size(54, 13)
        lblAddresse.TabIndex <- 6
        lblAddresse.Text <- "Addresse:"
        // 
        // txtAddresse
        // 
        txtAddresse.Location <- Point(123, 120)
        txtAddresse.Name <- "txtAddresse"
        txtAddresse.Size <- Size(100, 20)
        txtAddresse.TabIndex <- 7
        // 
        // lblDate
        // 
        lblDate.AutoSize <- true
        lblDate.Location <- Point(59, 146)
        lblDate.Name <- "lblDate"
        lblDate.Size <- Size(33, 13)
        lblDate.TabIndex <- 8
        lblDate.Text <- "Date:"
        // 
        // txtDate
        // 
        txtDate.Location <- Point(123, 146)
        txtDate.Name <- "txtDate"
        txtDate.Size <- Size(100, 20)
        txtDate.TabIndex <- 9
        // 
        // btnAjouter
        // 
        btnAjouter.Location <- Point(52, 188)
        btnAjouter.Name <- "btnAjouter"
        btnAjouter.Size <- Size(63, 23)
        btnAjouter.TabIndex <- 10
        btnAjouter.Text <- "Ajouter"
        btnAjouter.UseVisualStyleBackColor <- true
        btnAjouter.Click.AddHandler(new EventHandler(this.btnAjouter_Click))
        // 
        // btnModifier
        // 
        btnModifier.Location <- Point(129, 188)
        btnModifier.Name <- "btnModifier"
        btnModifier.Size <- Size(63, 23)
        btnModifier.TabIndex <- 11
        btnModifier.Text <- "Modifier"
        btnModifier.UseVisualStyleBackColor <- true
        btnModifier.Click.AddHandler(new EventHandler(this.btnModifier_Click))
        // 
        // btnSupprimer
        // 
        btnSupprimer.Location <- Point(206, 188)
        btnSupprimer.Name <- "btnSupprimer";
        btnSupprimer.Size <- Size(63, 23)
        btnSupprimer.TabIndex <- 12
        btnSupprimer.Text <- "Supprimer"
        btnSupprimer.UseVisualStyleBackColor <- true
        btnSupprimer.Click.AddHandler(new EventHandler(this.btnSupprimer_Click))
        //
        // btnPrecedant
        // 
        btnPrecedant.Location <- Point(89, 217)
        btnPrecedant.Name <- "btnPrecedant"
        btnPrecedant.Size <- Size(31, 23)
        btnPrecedant.TabIndex <- 13
        btnPrecedant.Text <- "<-"
        btnPrecedant.UseVisualStyleBackColor <- true
        // 
        // btnSuivant
        // 
        btnSuivant.Location <- Point(126, 217)
        btnSuivant.Name <- "btnSuivant"
        btnSuivant.Size <- Size(31, 23)
        btnSuivant.TabIndex <- 14
        btnSuivant.Text <- "->"
        btnSuivant.UseVisualStyleBackColor <- true
        // 
        // btnDernier
        // 
        btnDernier.Location <- Point(163, 217)
        btnDernier.Name <- "btnDernier"
        btnDernier.Size <- Size(31, 23)
        btnDernier.TabIndex <- 16
        btnDernier.Text <- ">>"
        btnDernier.UseVisualStyleBackColor <- true
        // 
        // btnPremier
        // 
        btnPremier.Location <- Point(52, 217)
        btnPremier.Name <- "btnPremier"
        btnPremier.Size <- Size(31, 23)
        btnPremier.TabIndex <- 15
        btnPremier.Text <- "<<"
        btnPremier.UseVisualStyleBackColor <- true
        // 
        // btnDel
        // 
        btnDel.Location <- Point(239, 217)
        btnDel.Name <- "btnDel"
        btnDel.Size <- Size(31, 23)
        btnDel.TabIndex <- 18
        btnDel.Text <- "-"
        btnDel.UseVisualStyleBackColor <- true
        // 
        // btnAdd
        // 
        btnAdd.Location <- Point(202, 217)
        btnAdd.Name <- "btnAdd"
        btnAdd.Size <- Size(31, 23)
        btnAdd.TabIndex <- 17
        btnAdd.Text <- "+"
        btnAdd.UseVisualStyleBackColor <- true
        // 
        // lblNotification
        // 
        lblNotification.AutoSize <- true
        lblNotification.Font <- new Font("Microsoft Sans Serif", 9.0f, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)))
        lblNotification.ForeColor <- Color.SteelBlue
        lblNotification.Location <- new Point(229, 43)
        lblNotification.Name <- "lblNotification"
        lblNotification.Size <- new Size(80, 15)
        lblNotification.TabIndex <- 19
        lblNotification.Text <- "Notification"
        // 
        // label1
        // 
        label1.AutoSize <- true
        label1.Location <- Point(59, 9)
        label1.Name <- "label1"
        label1.Size <- Size(53, 13)
        label1.TabIndex <- 20
        label1.Text <- "Chercher:"
        // 
        // txtChercher
        // 
        txtChercher.Location <- Point(123, 9)
        txtChercher.Name <- "txtChercher"
        txtChercher.Size <- Size(100, 20)
        txtChercher.TabIndex <- 21
        txtChercher.KeyPress.AddHandler(new KeyPressEventHandler(this.txtChercher_KeyPress))
        // 
        // Employee
        // 
        this.AutoScaleDimensions <- new System.Drawing.SizeF(6.0F, 13.0F)
        this.AutoScaleMode <- AutoScaleMode.Font
        this.ClientSize <- new Size(380, 275);
        this.Controls.Add(txtChercher)
        this.Controls.Add(label1)
        this.Controls.Add(lblNotification)
        this.Controls.Add(btnDel)
        this.Controls.Add(btnAdd)
        this.Controls.Add(btnDernier)
        this.Controls.Add(btnPremier)
        this.Controls.Add(btnSuivant)
        this.Controls.Add(btnPrecedant)
        this.Controls.Add(btnSupprimer)
        this.Controls.Add(btnModifier)
        this.Controls.Add(btnAjouter)
        this.Controls.Add(txtDate)
        this.Controls.Add(lblDate)
        this.Controls.Add(txtAddresse)
        this.Controls.Add(lblAddresse)
        this.Controls.Add(txtLastName)
        this.Controls.Add(lblLastName)
        this.Controls.Add(txtFirstName)
        this.Controls.Add(lblFirstName)
        this.Controls.Add(txtId)
        this.Controls.Add(lblId)
        this.Name <- "Employee"
        this.Text <- "Employee"
        this.ResumeLayout(false)
        this.PerformLayout()

        
        this.SuspendLayout() |> ignore

    
    member this.txtChercher_KeyPress sender eventArg = 
        if eventArg.KeyChar = char(13) then 
            mapper.Select (Convert.ToInt32(txtChercher.Text)) |> this.RefreshValues |> ignore
    
    member this.btnAjouter_Click sender eventArgs = 
        let employee = new Dictionary<string, obj>() 
        employee.Add("Id", txtId.Text)
        employee.Add("FirstName", txtFirstName.Text)
        employee.Add("LastName", txtLastName.Text)
        employee.Add("Addresse", txtAddresse.Text)
        employee.Add("Date", txtDate.Text)    
        
        mapper.Insert(employee) |> ignore

    member this.btnModifier_Click sender eventArgs = 
        let employee = new Dictionary<string, obj>() 
        employee.Add("Id", txtId.Text)
        employee.Add("FirstName", txtFirstName.Text)
        employee.Add("LastName", txtLastName.Text)
        employee.Add("Addresse", txtAddresse.Text)
        employee.Add("Date", txtDate.Text)   

        mapper.Update(employee) |> ignore
    
    member this.btnSupprimer_Click sender eventArgs = 
        mapper.Delete(Convert.ToInt32(txtId.Text)) |> ignore 

    member this.RefreshValues (v:Dictionary<string, obj>) =
        txtId.Text <- Convert.ToString(v.Item("FirstName"))
        txtFirstName.Text <- Convert.ToString(v.Item("FirstName"))
        txtLastName.Text <- Convert.ToString(v.Item("LastName"))
        txtAddresse.Text <- Convert.ToString(v.Item("Addresse"))
        txtDate.Text <- Convert.ToString(v.Item("Date"))