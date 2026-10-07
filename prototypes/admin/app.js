const titles = {
  today: ["Today", "Four transfer vouchers are waiting. The flood kitchens are three quarters funded."],
  campaigns: ["Campaigns", "Every live appeal, how full it is, and who is looking after it."],
  campaign: ["Campaign", "Write the appeal, then read it the way a donor will."],
  donations: ["Donations", "Gifts that have arrived, and the vouchers still waiting on a check."],
  donors: ["Donors", "The people behind the gifts, and whether their sign-in is open."],
  expenses: ["Expenses", "What a campaign has spent against what donors have already sent."],
  staff: ["Staff", "Who can sign in, and which role they hold."],
  setup: ["Setup", "Roles, campaign types, how money arrives, and how it is spent."]
};

const $ = (sel, root = document) => root.querySelector(sel);
const $$ = (sel, root = document) => [...root.querySelectorAll(sel)];

const q = $("#q");
let toastTimer;

function mmk(n) {
  return new Intl.NumberFormat("en-US").format(Math.round(n));
}

function prettyDate(iso) {
  if (!iso) return "Set a closing date";
  const [year, month, day] = iso.split("-").map(Number);
  const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
  return `Closes ${day} ${months[month - 1]} ${year}`;
}

let editingExpense = null;

function show(name) {
  $$("[data-screen]").forEach((screen) => {
    screen.hidden = screen.dataset.screen !== name;
  });
  $$(".nav [data-go]").forEach((button) => {
    const on = button.dataset.go === name || (name === "campaign" && button.dataset.go === "campaigns");
    button.classList.toggle("is-on", on);
  });
  const [title, lede] = titles[name];
  $("#title").textContent = title;
  $("#lede").textContent = lede;
  $("#eyebrow").textContent = name === "today" ? "Wednesday, 7 Oct" : "Operations";
  $$("[data-actions]").forEach((node) => {
    node.hidden = !node.dataset.actions.split(" ").includes(name);
  });
  document.title = `${titles[name][0]} — BetterTogether Operations`;
  paintRows($(`[data-screen="${name}"]`));
}

function paintRows(screen) {
  if (!screen) return;
  const query = q.value.trim().toLowerCase();
  const chip = screen.querySelector("[data-filter].is-on");
  const filter = chip ? chip.dataset.filter : "all";
  const rows = $$("[data-row]", screen).filter((row) => {
    const boxed = row.closest("[data-ledger], [data-profile], [data-pane]");
    return !boxed || !boxed.hidden;
  });
  rows.forEach((row) => {
    const statusOk = filter === "all" || row.dataset.status === filter;
    const hay = (row.dataset.search || row.textContent).toLowerCase();
    row.hidden = !(statusOk && (!query || hay.includes(query)));
  });
  const miss = screen.querySelector(":scope > .miss, .sheet .miss, .directory .miss");
  if (miss) {
    const visible = rows.some((row) => !row.hidden);
    miss.hidden = visible || rows.length === 0;
  }
}

function toast(message) {
  const node = $("#toast");
  node.textContent = message;
  node.hidden = false;
  clearTimeout(toastTimer);
  toastTimer = setTimeout(() => {
    node.hidden = true;
  }, 2400);
}

function pendingCount() {
  const count = $$('#donation-rows [data-row][data-status="pending"]').length;
  $("#pending-count").textContent = String(count);
  $("#awaiting-figure").textContent = String(count);
  return count;
}

function fillEditor(row) {
  const editor = $("#editor");
  const blank = !row;
  editor.dataset.raised = blank ? "0" : row.dataset.raised;
  editor.dataset.tone = blank ? "flood" : row.dataset.tone;
  editor.dataset.id = blank ? "" : row.dataset.id;
  $("#f-title").value = blank ? "" : row.dataset.title;
  $("#f-description").value = blank ? "" : row.dataset.description;
  $("#f-target").value = blank ? "" : row.dataset.target;
  $("#f-date").value = blank ? "" : row.dataset.date;
  $("#f-type").value = blank ? "Relief" : row.dataset.type;
  $("#f-owner").value = blank ? "Hnin Wai" : row.dataset.owner;
  $("#f-files").textContent = blank ? "" : "3 photos already attached";
  const status = blank ? "Draft" : statusLabel(row.dataset.status);
  $("#editor-status").textContent = status;
  $("#editor-status").className = `pill status ${blank ? "off" : statusClass(row.dataset.status)}`;
  $("#pause-campaign").hidden = blank;
  $("#pause-campaign").textContent = row && row.dataset.status === "paused" ? "Reopen campaign" : "Pause campaign";
  renderPreview();
}

function statusLabel(status) {
  if (status === "active") return "Active";
  if (status === "paused") return "Paused";
  if (status === "funded") return "Funded";
  if (status === "inactive") return "Inactive";
  if (status === "pending") return "Needs check";
  if (status === "verified") return "Verified";
  return status;
}

function statusClass(status) {
  if (status === "active" || status === "verified" || status === "funded") return "ok";
  if (status === "pending") return "wait";
  if (status === "paused" || status === "inactive") return "off";
  return "";
}

function renderPreview() {
  const title = $("#f-title").value.trim() || "Untitled campaign";
  const description = $("#f-description").value.trim() || "The story donors will read sits here.";
  const target = Number($("#f-target").value || 0);
  const raised = Number($("#editor").dataset.raised || 0);
  const pct = target > 0 ? Math.min(100, Math.round((raised / target) * 100)) : 0;
  $("#preview-title").textContent = title;
  $("#preview-desc").textContent = description;
  $("#preview-type").textContent = $("#f-type").value;
  $("#preview-raised").textContent = mmk(raised);
  $("#preview-target").textContent = target ? mmk(target) : "—";
  $("#preview-meter").style.width = `${pct}%`;
  $("#preview-meter").parentElement.classList.toggle("done", raised >= target && target > 0);
  $("#preview-date").textContent = prettyDate($("#f-date").value);
  const cover = $("#preview-cover");
  cover.className = `cover ${$("#editor").dataset.tone || "flood"}`;
}

function openSlip(row) {
  $("#slip-donor").textContent = row.dataset.donor;
  $("#slip-amount").textContent = row.dataset.amount;
  $("#slip-method").textContent = row.dataset.method;
  $("#slip-appeal").textContent = row.dataset.appeal;
  $("#slip-ref").textContent = row.dataset.ref;
  $("#slip-when").textContent = row.dataset.when;
  $("#slip-note").textContent = row.dataset.note || "—";
  const pending = row.dataset.status === "pending";
  $("#verify").hidden = !pending;
  $("#verify").dataset.id = row.dataset.id;
  $("#slip-state").textContent = pending ? "Waiting for a match" : "Already counted";
  $("#drawer").hidden = false;
  $("#backdrop").hidden = false;
}

function closeDrawer() {
  $("#drawer").hidden = true;
  $("#backdrop").hidden = true;
}

function closeModal() {
  $("#expense-modal").hidden = true;
}

function setPill(node, status) {
  node.textContent = statusLabel(status);
  node.className = `pill status ${statusClass(status)}`;
}

function renderLedger(section) {
  const received = Number(section.dataset.received);
  const spent = Number(section.dataset.spent);
  const target = Number(section.dataset.target);
  section.querySelector('[data-money="received"]').textContent = mmk(received);
  section.querySelector('[data-money="spent"]').textContent = mmk(spent);
  section.querySelector('[data-money="left"]').textContent = mmk(received - spent);
  section.querySelector('[data-money="expected"]').textContent = mmk(target - spent);
  const empty = section.querySelector(".empty-ledger");
  const rows = section.querySelectorAll("tbody tr");
  if (empty) empty.hidden = rows.length > 0;
}

const donorGifts = {
  su: [
    { date: "6 Oct 2026", appeal: "Ayeyarwady Flood Kitchens", amount: "500,000" },
    { date: "28 Sep 2026", appeal: "Ayeyarwady Flood Kitchens", amount: "200,000" }
  ],
  kyaw: [
    { date: "2 Oct 2026", appeal: "Mandalay Reading Rooms", amount: "250,000" }
  ],
  hla: [
    { date: "5 Oct 2026", appeal: "Shan Mobile Clinic", amount: "1,000,000" }
  ],
  min: [
    { date: "6 Oct 2026", appeal: "Dry Zone Wells", amount: "150,000" }
  ],
  ei: [
    { date: "4 Oct 2026", appeal: "Yangon Winter Blankets", amount: "80,000" }
  ]
};

function selectDonor(id) {
  const button = $(`#donor-list [data-donor="${id}"]`);
  if (!button) return;
  $$("#donor-list .person").forEach((item) => item.classList.toggle("is-on", item === button));
  $("#donor-avatar").textContent = button.dataset.initials;
  $("#donor-avatar").className = `avatar ${button.dataset.tone || ""}`.trim();
  $("#donor-name").textContent = button.dataset.name;
  setPill($("#donor-status"), button.dataset.status);
  $("#donor-email").textContent = button.dataset.email;
  $("#donor-phone").textContent = button.dataset.phone;
  $("#donor-address").textContent = button.dataset.address;
  $("#donor-given").textContent = `${button.dataset.given} MMK`;
  $("#donor-since").textContent = button.dataset.since;
  $("#donor-toggle").textContent = button.dataset.status === "active" ? "Deactivate sign-in" : "Activate sign-in";
  $("#see-gifts").dataset.find = button.dataset.name;
  const gifts = donorGifts[id] || [];
  $("#gift-list").innerHTML = gifts.length
    ? gifts.map((gift) => `<div class="gift"><span><strong>${gift.appeal}</strong><div class="meta">${gift.date}</div></span><span class="money">${gift.amount}</span></div>`).join("")
    : `<p class="meta">No gifts yet.</p>`;
}

function selectStaff(button) {
  $$("#staff-list .person").forEach((item) => item.classList.toggle("is-on", item === button));
  $("#staff-id").value = button.dataset.id;
  $("#s-first").value = button.dataset.first;
  $("#s-last").value = button.dataset.last;
  $("#s-email").value = button.dataset.email;
  $("#s-address").value = button.dataset.address;
  $("#s-phone").value = button.dataset.phone;
  $("#s-role").value = button.dataset.role;
  $("#s-password").value = "";
  $("#staff-form-title").textContent = `${button.dataset.first} ${button.dataset.last}`;
  $("#staff-password-hint").textContent = "Leave blank to keep the current password.";
  $("#staff-toggle").textContent = button.dataset.status === "active" ? "Deactivate" : "Activate";
  $("#staff-toggle").hidden = false;
}

document.body.addEventListener("click", (event) => {
  const go = event.target.closest("[data-go]");
  if (go) {
    if (go.dataset.blank === "1") fillEditor(null);
    show(go.dataset.go);
  }

  const campaignBtn = event.target.closest("[data-open-campaign]");
  if (campaignBtn) {
    const row = $(`#campaign-rows [data-id="${campaignBtn.dataset.openCampaign}"]`);
    fillEditor(row);
    show("campaign");
  }

  const filter = event.target.closest("[data-filter]");
  if (filter) {
    $$("[data-filter]", filter.parentElement).forEach((chip) => chip.classList.toggle("is-on", chip === filter));
    paintRows(filter.closest("[data-screen]"));
  }

  const review = event.target.closest("[data-review]");
  if (review) {
    const row = $(`#donation-rows [data-id="${review.dataset.review}"]`);
    if (row) openSlip(row);
  }

  const gifts = event.target.closest("[data-find]");
  if (gifts) {
    q.value = gifts.dataset.find;
    show("donations");
  }

  const donor = event.target.closest("#donor-list .person");
  if (donor) selectDonor(donor.dataset.donor);

  const staffBtn = event.target.closest("#staff-list .person");
  if (staffBtn && !event.target.closest("[data-staff-toggle]")) selectStaff(staffBtn);

  const ledgerBtn = event.target.closest("[data-ledger-pick]");
  if (ledgerBtn) {
    $$("[data-ledger-pick]").forEach((button) => button.classList.toggle("is-on", button === ledgerBtn));
    $$("[data-ledger]").forEach((section) => {
      section.hidden = section.dataset.ledger !== ledgerBtn.dataset.ledgerPick;
    });
    $("#expense-campaign").value = ledgerBtn.dataset.ledgerPick;
    paintRows(ledgerBtn.closest("[data-screen]"));
  }

  const tab = event.target.closest("[data-tab]");
  if (tab) {
    $$("[data-tab]").forEach((button) => button.classList.toggle("is-on", button === tab));
    $$("[data-pane]").forEach((pane) => {
      pane.hidden = pane.dataset.pane !== tab.dataset.tab;
    });
    paintRows(tab.closest("[data-screen]"));
  }

  const reveal = event.target.closest("[data-reveal]");
  if (reveal) {
    const form = reveal.closest("[data-pane]").querySelector("[data-setup-form]");
    form.hidden = false;
    delete form.dataset.editing;
    form.reset();
    form.querySelector("[name=name]").focus();
  }

  const edit = event.target.closest("[data-edit]");
  if (edit) {
    const row = edit.closest("[data-row]");
    const form = row.closest("[data-pane]").querySelector("[data-setup-form]");
    form.hidden = false;
    form.dataset.editing = row.dataset.id;
    form.querySelector("[name=name]").value = row.querySelector("[data-label]").textContent.trim();
    const note = form.querySelector("[name=note]");
    if (note) note.value = row.dataset.note || "";
    const check = form.querySelector("[name=verify]");
    if (check) check.checked = row.dataset.verify === "yes";
    form.querySelector("[name=name]").focus();
  }

  const expenseEdit = event.target.closest("[data-expense-edit]");
  if (expenseEdit) {
    const row = expenseEdit.closest("tr");
    editingExpense = row;
    $("#x-title").value = row.querySelector("strong").textContent.trim();
    $("#x-note").value = row.querySelector(".meta")?.textContent.trim() || "";
    $("#x-amount").value = Number(row.querySelector(".money").textContent.replace(/,/g, ""));
    $("#x-type").value = row.children[1].textContent.trim();
    $("#expense-campaign").value = row.closest("[data-ledger]").dataset.ledger;
    $("#expense-form button[type=submit]").textContent = "Save expense";
    $("#expense-modal").hidden = false;
    $("#x-title").focus();
  }

  const toggle = event.target.closest("[data-toggle-row]");
  if (toggle) {
    const row = toggle.closest("[data-row]");
    const next = row.dataset.status === "active" ? "inactive" : "active";
    row.dataset.status = next;
    setPill(row.querySelector(".status"), next);
    toggle.textContent = next === "active" ? "Deactivate" : "Activate";
    toast(`${row.querySelector("[data-label]").textContent.trim()} is ${statusLabel(next).toLowerCase()}.`);
  }

  if (event.target.closest("[data-close-drawer]")) closeDrawer();
  if (event.target.id === "backdrop" || event.target.id === "expense-modal") {
    closeDrawer();
    closeModal();
  }
  if (event.target.closest("[data-close-modal]")) closeModal();
});

$("#verify").addEventListener("click", () => {
  const id = $("#verify").dataset.id;
  $$(`[data-id="${id}"]`).forEach((node) => {
    if (!node.dataset.status) return;
    node.dataset.status = "verified";
    const pill = node.querySelector(".status");
    if (pill) setPill(pill, "verified");
    const button = node.querySelector("[data-review]");
    if (button) button.textContent = "Voucher";
  });
  pendingCount();
  closeDrawer();
  toast("Voucher matched. The gift now counts, and the donor can collect a certificate.");
  paintRows($("[data-screen='donations']"));
});

$("#campaign-rows").addEventListener("click", (event) => {
  if (event.target.closest("button")) return;
  const row = event.target.closest("[data-open]");
  if (!row) return;
  fillEditor(row);
  show("campaign");
});

["#f-title", "#f-description", "#f-target", "#f-date", "#f-type"].forEach((id) => {
  $(id).addEventListener("input", renderPreview);
});

$("#f-photos").addEventListener("change", (event) => {
  const names = [...event.target.files].map((file) => file.name);
  $("#f-files").textContent = names.length ? names.join(", ") : "";
});

$("#save-campaign").addEventListener("click", () => {
  const title = $("#f-title").value.trim();
  if (!title) {
    toast("Give the campaign a title before saving.");
    $("#f-title").focus();
    return;
  }
  const id = $("#editor").dataset.id;
  if (id) {
    const row = $(`#campaign-rows [data-id="${id}"]`);
    row.dataset.title = title;
    row.dataset.description = $("#f-description").value;
    row.dataset.target = $("#f-target").value;
    row.dataset.date = $("#f-date").value;
    row.dataset.type = $("#f-type").value;
    row.dataset.owner = $("#f-owner").value;
    row.querySelector("[data-label]").textContent = title;
    row.querySelector("[data-type]").textContent = $("#f-type").value;
    row.querySelector("[data-owner]").textContent = $("#f-owner").value;
    const target = Number($("#f-target").value || 0);
    const raised = Number(row.dataset.raised || 0);
    row.querySelector("[data-progress]").textContent = `${mmk(raised)} of ${mmk(target)}`;
    const pct = target ? Math.min(100, Math.round((raised / target) * 100)) : 0;
    row.querySelector(".meter > span").style.width = `${pct}%`;
  }
  toast(id ? `Saved ${title}.` : `${title} is drafted on this desk. Connect the app to keep it.`);
});

$("#pause-campaign").addEventListener("click", () => {
  const id = $("#editor").dataset.id;
  const row = $(`#campaign-rows [data-id="${id}"]`);
  if (!row) return;
  const next = row.dataset.status === "paused" ? (Number(row.dataset.raised) >= Number(row.dataset.target) ? "funded" : "active") : "paused";
  row.dataset.status = next;
  setPill(row.querySelector(".status"), next);
  fillEditor(row);
  toast(`${row.dataset.title} is ${statusLabel(next).toLowerCase()}.`);
});

$("#donor-toggle").addEventListener("click", () => {
  const button = $("#donor-list .person.is-on");
  if (!button) return;
  const next = button.dataset.status === "active" ? "inactive" : "active";
  button.dataset.status = next;
  setPill(button.querySelector(".status"), next);
  setPill($("#donor-status"), next);
  $("#donor-toggle").textContent = next === "active" ? "Deactivate sign-in" : "Activate sign-in";
  toast(`${button.dataset.name} is ${statusLabel(next).toLowerCase()}.`);
});

$("#staff-list").addEventListener("click", (event) => {
  const toggle = event.target.closest("[data-staff-toggle]");
  if (!toggle) return;
  event.stopPropagation();
  const button = toggle.closest(".person");
  const next = button.dataset.status === "active" ? "inactive" : "active";
  button.dataset.status = next;
  setPill(button.querySelector(".status"), next);
  toggle.textContent = next === "active" ? "Deactivate" : "Activate";
  if (button.classList.contains("is-on")) {
    $("#staff-toggle").textContent = next === "active" ? "Deactivate" : "Activate";
  }
});

$("#new-staff").addEventListener("click", () => {
  $$("#staff-list .person").forEach((item) => item.classList.remove("is-on"));
  $("#staff-form").reset();
  $("#staff-id").value = "";
  $("#staff-form-title").textContent = "New staff";
  $("#staff-password-hint").textContent = "Set a password so they can sign in.";
  $("#staff-toggle").hidden = true;
  $("#s-first").focus();
});

$("#staff-form").addEventListener("submit", (event) => {
  event.preventDefault();
  const first = $("#s-first").value.trim();
  const last = $("#s-last").value.trim();
  if (!first || !last || !$("#s-email").value.trim()) {
    toast("First name, last name, and email are required.");
    return;
  }
  const id = $("#staff-id").value;
  if (id) {
    const button = $(`#staff-list [data-id="${id}"]`);
    button.dataset.first = first;
    button.dataset.last = last;
    button.dataset.email = $("#s-email").value.trim();
    button.dataset.address = $("#s-address").value.trim();
    button.dataset.phone = $("#s-phone").value.trim();
    button.dataset.role = $("#s-role").value;
    button.querySelector("[data-label]").textContent = `${first} ${last}`;
    button.querySelector("[data-role]").textContent = $("#s-role").value;
    $("#staff-form-title").textContent = `${first} ${last}`;
    toast(`Saved ${first} ${last}.`);
    return;
  }
  toast(`${first} ${last} is drafted. Connect the app to create the account.`);
});

$("#staff-toggle").addEventListener("click", () => {
  const id = $("#staff-id").value;
  const button = $(`#staff-list [data-id="${id}"]`);
  if (!button) return;
  button.querySelector("[data-staff-toggle]").click();
});

$("#add-expense").addEventListener("click", () => {
  editingExpense = null;
  $("#expense-form").reset();
  $("#expense-form button[type=submit]").textContent = "Add to the ledger";
  const current = $(".switcher .is-on");
  $("#expense-campaign").value = current.dataset.ledgerPick;
  $("#expense-modal").hidden = false;
  $("#x-title").focus();
});

$("#expense-form").addEventListener("submit", (event) => {
  event.preventDefault();
  const title = $("#x-title").value.trim();
  const amount = Number($("#x-amount").value);
  const type = $("#x-type").value;
  const campaign = $("#expense-campaign").value;
  if (!title || !amount) {
    toast("Add a title and an amount.");
    return;
  }
  const note = $("#x-note").value.trim();
  const section = $(`[data-ledger="${campaign}"]`);
  if (editingExpense) {
    const oldAmount = Number(editingExpense.querySelector(".money").textContent.replace(/,/g, "")) || 0;
    const oldSection = editingExpense.closest("[data-ledger]");
    oldSection.dataset.spent = String(Number(oldSection.dataset.spent) - oldAmount);
    renderLedger(oldSection);
    editingExpense.dataset.search = `${title} ${type} ${note}`;
    editingExpense.innerHTML = `<td><strong>${title}</strong><div class="meta">${note || "Updated"}</div></td><td>${type}</td><td class="money out">${mmk(amount)}</td><td><button class="btn small quiet" type="button" data-expense-edit>Edit</button></td>`;
    section.querySelector("tbody").prepend(editingExpense);
    editingExpense = null;
    toast(`Saved ${title}.`);
  } else {
    const tr = document.createElement("tr");
    tr.dataset.row = "";
    tr.dataset.search = `${title} ${type} ${note}`;
    tr.innerHTML = `<td><strong>${title}</strong><div class="meta">${note || "Added just now"}</div></td><td>${type}</td><td class="money out">${mmk(amount)}</td><td><button class="btn small quiet" type="button" data-expense-edit>Edit</button></td>`;
    section.querySelector("tbody").prepend(tr);
    toast(`${title} added to the ledger.`);
  }
  section.dataset.spent = String(Number(section.dataset.spent) + amount);
  renderLedger(section);
  $(`[data-ledger-pick="${campaign}"]`).click();
  event.currentTarget.reset();
  $("#expense-form button[type=submit]").textContent = "Add to the ledger";
  closeModal();
});

$$("[data-setup-form]").forEach((form) => {
  form.addEventListener("submit", (event) => {
    event.preventDefault();
    const name = form.querySelector("[name=name]").value.trim();
    if (!name) {
      toast("Add a name first.");
      return;
    }
    const pane = form.closest("[data-pane]");
    const tbody = pane.querySelector("tbody");
    const noteInput = form.querySelector("[name=note]");
    const note = noteInput ? noteInput.value.trim() : "";
    const check = form.querySelector("[name=verify]");
    if (form.dataset.editing) {
      const row = tbody.querySelector(`[data-id="${form.dataset.editing}"]`);
      row.querySelector("[data-label]").textContent = name;
      row.dataset.search = `${name} ${note}`;
      if (note && row.querySelector("[data-note]")) row.querySelector("[data-note]").textContent = note;
      if (check && row.querySelector("[data-verify]")) {
        row.dataset.verify = check.checked ? "yes" : "no";
        row.querySelector("[data-verify]").textContent = check.checked ? "Voucher required" : "Instant";
      }
      toast(`Saved ${name}.`);
    } else {
      const id = `new-${Date.now()}`;
      const tr = document.createElement("tr");
      tr.dataset.row = "";
      tr.dataset.id = id;
      tr.dataset.status = "active";
      tr.dataset.search = name;
      tr.dataset.note = note;
      const extra = pane.dataset.pane === "payments"
        ? `<td data-note>${note || "—"}</td><td data-verify>${check && check.checked ? "Voucher required" : "Instant"}</td>`
        : pane.dataset.pane === "roles"
          ? `<td>0</td>`
          : pane.dataset.pane === "types"
            ? `<td>0</td>`
            : "";
      tr.innerHTML = `<td data-label>${name}</td>${extra}<td><span class="pill status ok">Active</span></td><td><button class="btn small quiet" type="button" data-edit>Edit</button> <button class="btn small danger" type="button" data-toggle-row>Deactivate</button></td>`;
      tbody.prepend(tr);
      toast(`Added ${name}.`);
    }
    form.reset();
    form.hidden = true;
    delete form.dataset.editing;
  });
});

q.addEventListener("input", () => {
  const screen = $("[data-screen]:not([hidden])");
  paintRows(screen);
});

document.addEventListener("keydown", (event) => {
  if (event.key === "Escape") {
    closeDrawer();
    closeModal();
  }
});

$$("[data-ledger]").forEach(renderLedger);
fillEditor($("#campaign-rows [data-id='flood']"));
selectDonor("su");
selectStaff($("#staff-list [data-id='hw']"));
show("today");

const requested = new URLSearchParams(location.search);
if (requested.get("screen") && titles[requested.get("screen")]) {
  if (requested.get("screen") === "campaign" && requested.get("id")) {
    const row = $(`#campaign-rows [data-id="${requested.get("id")}"]`);
    if (row) fillEditor(row);
  }
  show(requested.get("screen"));
}
if (requested.get("drawer")) {
  const slip = $(`#donation-rows [data-id="${requested.get("drawer")}"]`);
  if (slip) openSlip(slip);
}
