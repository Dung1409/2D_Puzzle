# Game Design Document: 2D_Puzzel

| Field | Value |
|---|---|
| **Project Name** | 2D_Puzzel |
| **Version** | 1.0 |
| **Platform** | Android / iOS / PC (Unity) |
| **Target Audience** | Casual mobile gamers, 12–45, both genders |
| **Estimated TTH (Time to Hollow)** | 3–5 phút/session |

---

## 1. Game Overview

### Elevator Pitch
Một game block puzzle kéo-thả 2D, nơi người chơi sắp xếp các khối đa dạng lên lưới để tạo thành hàng và cột đầy đủ, xoá chúng và tích điểm. Không giới hạn thời gian, chỉ cần bạn còn nước đi.

### Core Fantasy
> "Cảm giác thư giãn khi kéo những khối màu vào đúng vị trí, và hài lòng khi cả hàng biến mất trong một nhịp."

### Unique Selling Points
1. **Drag-and-Drop mượt** — kéo thả trực tiếp, feedback real-time màu sắc (xanh = OK, đỏ = lỗi) ngay trên block.
2. **Shape đa dạng** — định nghĩa shape dạng ma trận bool qua ScriptableObject, dễ dàng thêm hàng trăm shape mà không cần code.
3. **Request Shape** — 3 lượt/ván để đổi shape mới, tạo chiến thuật và cứu thua.

---

## 2. Core Gameplay

### Core Loop
```
[Shape xuất hiện] → [Kéo thả vào lưới] → [Hàng/Cột đầy → Xoá] → [Score +] → [Shape mới xuất hiện]
                                 ↑                                         ↓
                                 └────────── [Hết nước đi → Game Over] ←───┘
```

### Session Flow
| Phase | Mô tả |
|---|---|
| **Menu** | Nhấn Space/Click để bắt đầu |
| **Playing** | Kéo thả shape, xoá hàng/cột, ghi điểm |
| **Game Over** | Popup hiện best score, chọn Try Again hoặc Back to Home |
| **Timer (nếu không chọn)** | 3s countdown → tự động về menu |

### Win/Lose Conditions
- **Lose**: Không còn shape nào có thể đặt vào bất kỳ vị trí nào trên lưới.
- **Win**: Không có win condition (endless score chasing). Best score là mục tiêu.

---

## 3. Progression & Difficulty

### Player Progression
- **Best Score** — mục tiêu duy nhất, lưu qua PlayerPrefs.
- **Request Count** — 3 lượt request/ván (lưu qua PlayerPrefs).

### Difficulty Curve
Không có difficulty curve tuyến tính. Độ khó phụ thuộc vào:
- Kích thước lưới (5×5 dễ → 10×10 khó hơn vì shape to chiếm nhiều ô).
- Shape pool càng nhiều shape dị dạng → khó hơn.

### Unlock System
*Chưa có — đề xuất ở phần mở rộng.*

---

## 4. Systems Design

### 4.1 Board System
- **Lưới**: 5×5 đến 10×10 (config qua Inspector).
- **Tile**: Mỗi ô có 2 child: hide tile (mask) + shape block (nếu có).
- **Index**: `Dictionary<Vector2Int, GameObject>` ánh xạ vị trí → tile.
- **Màu nền**: Xen kẽ normal/highlight tạo bàn cờ.

### 4.2 Shape System
- **ShapeData** (`ScriptableObject`): ma trận bool, row × column.
- **Storage**: `List<ShapeData>` load từ Resources/ShapeData.
- **Generate**: Random shape từ storage, tối đa 3 shape cùng lúc.
- **Color**: Random từ SquaresColor, tránh màu RED (reserved cho error).

### 4.3 Drag & Drop
| Event | Hành vi |
|---|---|
| OnPointerDown | Phóng to 1.25x, lưu vị trí |
| OnBeginDrag | Reset anchor/pivot cho từng block |
| OnDrag | Move theo chuột |
| OnPointerUp | Check từng block `canDrop`, phục hồi anchor |
| OnEndDrag | Hợp lệ → gắn vào tile. Không hợp lệ → về vị trí cũ |

### 4.4 Line Clear System
- Khi tile đầy (childCount == 2) → check hàng và cột.
- Duyệt từ `Vector2Int.right` và `Vector2Int.up`.
- Nếu tất cả tile trong line đều có shape → clear all.
- Score bonus cho multi-line clear.

### 4.5 Scoring
| Action | Points |
|---|---|
| Mỗi block đặt | +5 |
| 1 line (hàng/cột) | +10 |
| 2 lines (hàng + cột) | +20 |

### 4.6 Save/Load
- **Storage**: PlayerPrefs.
- **Save**: Khi thoát PlayMode (chưa game over) hoặc Back to Home.
- **Load**: Khi scene Awake.
- **Data**: BoardData (JSON serialization), Score, Shape slot index.

---

## 5. Content Design

### Shape Library
Hiện tại đang dùng shapeScriptableObject asset. Các shape mẫu nên có:

| Shape | Kích thước | Ghi chú |
|---|---|---|
| 1×1 | 1×1 | Cơ bản nhất |
| 2×1 / 1×2 | 2×1 / 1×2 | Line ngắn |
| 3×1 / 1×3 | 3×1 / 1×3 | Line dài |
| 2×2 | 2×2 | Square |
| 3×3 L | 3×3 | L-shape (7 ô) |
| T-shape | 3×3 | 5 ô |
| Z-shape | 3×3 | 4–5 ô |
| Custom | variable | Các shape đặc biệt |

Khuyến nghị: 20–30 shape cho bản full.

### Visual Style
- **Blocks**: Sprite màu flat, nhiều màu (trừ RED).
- **Board**: Background grid màu pastel.
- **Feedback**: RED overlay khi lỗi, trắng sáng khi hover hợp lệ.
- **Animation** (hiện có): Animator cho Writings (notification text khi clear line).

---

## 6. Monetization (Đề xuất)

| Loại | Mô tả | Fit |
|---|---|---|
| **Rewarded Ads** | Xem quảng cáo → +3 Request Shape | ★★★★★ |
| **Banner Ads** | Banner dưới màn hình | ★★★★☆ |
| **IAP Remove Ads** | Gỡ toàn bộ quảng cáo ($1.99) | ★★★★☆ |
| **Cosmetic Skins** | Theme màu cho board/block ($0.99) | ★★★☆☆ |

*Ưu tiên: Rewarded Ads → IAP Remove Ads, tránh gacha/pay-to-win để giữ casual audience.*

---

## 7. Retention & Live-Ops (Đề xuất)

| Feature | Mục đích |
|---|---|
| **Daily Challenge** | Lưới + shape pool cố định mỗi ngày, ai điểm cao nhất |
| **Achievement System** | "Xoá 1000 block", "Đạt 5000 điểm" |
| **Endless Mode** | Giữ nguyên luật chơi, countdown timer tạo áp lực |
| **Leaderboard** | Game Center / Google Play Services |

---

## 8. Technical Notes

| Item | Ghi chú |
|---|---|
| **Rendering** | 2D, URP (hiện tại đang dùng built-in) |
| **Input** | Mouse + Touch (Unity EventSystem) |
| **Save System** | PlayerPrefs — nên nâng cấp lên JSON file + encryption |
| **Analytics** | Chưa có — nên gắn sự kiện: `game_start`, `shape_placed`, `line_clear`, `game_over` |
| **Performance** | ObjectPooling cho block, không Instantiate/Destroy liên tục |

### Điểm yếu kỹ thuật hiện tại
1. **Singleton không thread-safe** — không vấn đề với game đơn luồng.
2. **Observer dùng delegate static** — có thể gây memory leak nếu không remove listener đúng.
3. **PlayerPrefs plaintext** — dễ bị hack điểm. Nên dùng `PlayerPrefs.SetString` với encode nhẹ.

---

## 9. MVP Scope

### Must Have (v1.0)
- Lưới 5×5 + 10 shape cơ bản
- Kéo thả + line clear + score
- Game Over + Best Score
- Menu scene
- Request Shape (3 lượt)

### Nice To Have
- Animation clear line
- Sound FX + BGM
- Haptic feedback
- Difficulty select (grid size)

### Future Features
- Daily Challenge
- Leaderboard
- IAP / Rewarded Ads
- Skin/themes
- Undo button
- Hint system

---

## 10. Design Review

| Tiêu chí | Score (1–10) | Ghi chú |
|---|---|---|
| **Market Fit** | 7 | Thể loại block puzzle đã chứng minh (1010!, Block Puzzle), nhưng cạnh tranh cao |
| **Innovation** | 4 | Drag-and-drop là cải tiến nhỏ so với tap-to-place; cần thêm USP |
| **Production Risk** | 3 | Codebase đơn giản, không phụ thuộc backend, dễ ship |
| **Monetization Potential** | 6 | Casual audience trả tiền thấp, nhưng rewarded ads là nguồn stable |

### 3 Features Để Tạo Khác Biệt

1. **🔮 Shape Preview + Ghost Block** — Khi kéo shape, hiển thị ghost (trong suốt) tại vị trí sẽ đặt trước khi thả — giảm cognitive load, tăng UX mượt.
2. **⚡ Combo Chain** — Xoá 2+ line liên tiếp trong 1 lần đặt → nhân điểm (×2, ×3, ×5). HUD hiệu ứng combo tạo cảm giác thoả mãn.
3. **🎨 Color Match Bonus** — Block cùng màu với tile nền → +2 điểm thưởng. Tile nền random màu mỗi ván, tạo layer chiến thuật mới.

---

## 11. Next Development Steps

| Step | Task | Priority |
|---|---|---|
| 1 | Thêm ghost preview khi kéo shape | P0 |
| 2 | Animation clear line (fade + particle) | P0 |
| 3 | Sound FX + BGM | P1 |
| 4 | Combo chain scoring | P1 |
| 5 | Color match bonus | P2 |
| 6 | Daily Challenge mode | P2 |
| 7 | Rewarded Ads integration | P2 |
| 8 | Leaderboard | P3 |
