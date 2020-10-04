import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { AuthService, User } from '../services/auth.service';
import { ChatMessage, ChatService } from '../services/chat.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  currentUser: User;

  faPaperPlane = faPaperPlane;

  messageForm = new FormGroup({
    message: new FormControl('')
  })

  messages: ChatMessage[];

  constructor(private authService: AuthService,
    private chatService: ChatService,
    private router: Router) {
    this.messages = [];
  }

  ngOnInit(): void {
    this.messages = [];
    this.currentUser = this.authService.getCurrentUser();

    this.chatService.startConnection().then(() => {
      console.log('connections started...');
    }).catch(err => {
      console.error(err);
    });

    this.chatService.setupRecieveMessageHook((data) => this.recievedMessage(data));

    this.getLast50Messages();
  }

  sendMessage() {
    const { message } = this.messageForm.value;

    if (!message) return;

    this.chatService.sendMessage({
      message,
      userName: this.currentUser.userName,
    });

    this.messageForm.setValue({ message: '' });
  }

  recievedMessage(message: ChatMessage) {
    this.messages.push(message);
  }

  getLast50Messages() {
    this.chatService.getLast50Messages().subscribe(messages => {
      this.messages = messages;
    });
  }

  doLogout() {
    this.authService.logOut();
    this.router.navigate(['login']);
  }
}
