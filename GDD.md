# GAME DESIGN DOCUMENT

## Project Information

| Item            | Description                     |
| --------------- | ------------------------------- |
| Project Name    | 2D Puzzle                       |
| Genre           | Puzzle / Casual                 |
| Platform        | Android, iOS, PC                |
| Target Audience | Người chơi casual từ 12–45 tuổi |
| Session Length  | 3–5 phút                        |

---

# 1. Game Overview

## Game Concept

2D Puzzle là một trò chơi xếp khối theo lượt, nơi người chơi kéo thả các khối hình khác nhau lên bàn chơi để hoàn thành hàng hoặc cột. Khi một hàng hoặc cột được lấp đầy, chúng sẽ bị xóa và người chơi nhận điểm thưởng.

Trò chơi không có giới hạn thời gian. Ván chơi chỉ kết thúc khi người chơi không còn vị trí hợp lệ để đặt các khối hiện có.

## Core Experience

Mang đến trải nghiệm thư giãn, dễ tiếp cận và phù hợp với mọi đối tượng người chơi, đồng thời khuyến khích tư duy sắp xếp và quản lý không gian hiệu quả.

## Key Features

* Kéo thả khối trực tiếp lên bàn chơi.
* Hệ thống shape đa dạng.
* Xóa hàng và cột để ghi điểm.
* Lưu điểm cao nhất giữa các phiên chơi.
* Tính năng đổi shape giới hạn số lần sử dụng.

---

# 2. Core Gameplay

## Gameplay Loop

1. Nhận các shape ngẫu nhiên.
2. Kéo thả shape lên bàn chơi.
3. Hoàn thành hàng hoặc cột để xóa chúng.
4. Nhận điểm thưởng.
5. Tiếp tục cho đến khi không còn nước đi.

## Win / Lose Condition

### Win Condition

Không có điều kiện chiến thắng. Người chơi cố gắng đạt điểm số cao nhất có thể.

### Lose Condition

Trò chơi kết thúc khi không còn vị trí hợp lệ để đặt bất kỳ shape nào đang có.

---

# 3. Core Systems

## Board System

* Bàn chơi dạng lưới vuông.
* Kích thước có thể thay đổi tùy phiên bản.
* Mỗi ô chỉ chứa tối đa một block.

## Shape System

Các shape được tạo ngẫu nhiên từ thư viện shape có sẵn.

Ví dụ:

* 1×1
* 1×2
* 2×2
* L Shape
* T Shape
* Z Shape

## Drag & Drop System

Người chơi có thể:

* Chọn shape.
* Kéo shape đến vị trí mong muốn.
* Thả shape để đặt vào bàn chơi.

Nếu vị trí hợp lệ, shape sẽ được đặt xuống. Nếu không hợp lệ, shape sẽ trở về vị trí ban đầu.

## Line Clear System

Sau khi đặt shape:

* Hàng đầy sẽ bị xóa.
* Cột đầy sẽ bị xóa.
* Người chơi nhận điểm thưởng tương ứng.

---

# 4. Scoring System

| Action                  | Points     |
| ----------------------- | ---------- |
| Xóa 1 line              | +10        |
| Xóa nhiều line cùng lúc | Bonus điểm |

Mục tiêu chính của người chơi là đạt được điểm số cao nhất.

---

# 5. Progression

## Best Score

Điểm cao nhất được lưu lại để người chơi có mục tiêu vượt qua trong các lần chơi tiếp theo.

## Request Shape

Mỗi ván người chơi có một số lượt đổi shape nhất định.

Tính năng này giúp giảm yếu tố may rủi và tạo thêm lựa chọn chiến thuật.

---

# 6. Visual Direction

## Art Style

* Đồ họa 2D tối giản.
* Màu sắc tươi sáng.
* Giao diện thân thiện và dễ nhìn trên thiết bị di động.

## Feedback

* Hiển thị trạng thái hợp lệ khi kéo shape.
* Hiệu ứng khi xóa hàng hoặc cột.
* Hiệu ứng hiển thị điểm số.

---

# 7. MVP Scope

## Must Have

* Board System
* Shape System
* Drag & Drop
* Line Clear
* Score System
* Best Score Save
* Game Over Screen

## Nice To Have

* Hiệu ứng âm thanh
* Haptic Feedback
* Animation xóa line
* Nhiều kích thước bàn chơi

---

# 8. Future Improvements

### Ghost Placement Preview

Hiển thị vị trí dự kiến của shape trước khi thả.

### Combo System

Thưởng điểm khi người chơi liên tục tạo line clear.

### Daily Challenge

Cung cấp thử thách mới mỗi ngày với mục tiêu điểm số riêng.

### Achievement System

Bổ sung hệ thống thành tích để tăng động lực chơi lâu dài.

### Leaderboard

Cho phép người chơi cạnh tranh điểm số với những người chơi khác trên bảng xếp hạng trực tuyến.
