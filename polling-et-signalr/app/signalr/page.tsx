"use client";

import React from "react";
import { useEffect } from "react";
import { UselessTask } from "../models/UselessTask";
import TaskView from "../_components/tasks-view";
import * as signalR from "@microsoft/signalr";


export default function Home() {

  const [tasks, setTasks] = React.useState<UselessTask[]>([]);
  const [hubConnection, setHubConnection] = React.useState<signalR.HubConnection>();
  const [usercount, setUserCount] = React.useState<number>(0);


  useEffect(() => {
      // eslint-disable-next-line react-hooks/immutability
      connecttohub();
    }, []);

  function connecttohub() {
    // TODO On doit commencer par créer la connexion vers le Hub
    const newHubConnection = new signalR.HubConnectionBuilder().withUrl('http://localhost:5042/UselessTasksHub').build();
    // TODO On peut commencer à écouter pour les évènements qui vont déclencher des callbacks
    newHubConnection.on('TaskList', (data) => {
      setTasks(data);
      console.log(data);
    });
    newHubConnection.on('UserCount', (data) => {
      setUserCount(data);
    });
    // TODO On doit ensuite se connecter
    newHubConnection.start().then(() => {
      console.log('La connexion est active!');
    })
    .catch(err => console.log('Error while starting connection: ' + err));

    setHubConnection(newHubConnection);
  }

  function onTaskToggle(id: number) {
    // TODO On invoke la méthode pour compléter une tâche sur le serveur
    hubConnection?.invoke('CompleteTask', id)
  }

  function handleTaskAdd(taskName: string) {
    // TODO On invoke la méthode pour ajouter une tâche sur le serveur
    hubConnection?.invoke('AddTask', taskName)
  }

  return (
    <div className="p-4">
        <h1>SignalR!</h1>
        <TaskView 
          tasks={tasks} 
          onTaskAdd={handleTaskAdd}
          onTaskToggle={onTaskToggle}
        />
      <p>Nombre d&apos;utilisateurs connectés: {usercount}</p>
    </div>
  );
}