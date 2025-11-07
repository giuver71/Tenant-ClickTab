import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopbarComponentComponent } from './topbar.component';

describe('TopbarComponentComponent', () => {
  let component: TopbarComponentComponent;
  let fixture: ComponentFixture<TopbarComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopbarComponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TopbarComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
