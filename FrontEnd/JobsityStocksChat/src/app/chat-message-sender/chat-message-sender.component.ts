import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { ChatMessage } from '../services/chat.service';

@Component({
  selector: 'app-chat-message-sender',
  templateUrl: './chat-message-sender.component.html',
  styleUrls: ['./chat-message-sender.component.css']
})
export class ChatMessageSenderComponent implements OnInit, AfterViewInit {

  @Input()
  message: ChatMessage;
  container: HTMLElement;

  constructor() { }

  ngOnInit(): void {
  }


  ngAfterViewInit(): void {
    this.container = document.getElementById("messages-container");
    this.container.scrollTop = this.container.scrollHeight;
  }

}
