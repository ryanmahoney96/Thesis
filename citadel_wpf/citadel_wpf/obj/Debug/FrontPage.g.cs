﻿#pragma checksum "..\..\FrontPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "55FC91D083C52429CF3C641A4A1A7F94442D908D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using citadel_wpf;


namespace citadel_wpf {
    
    
    /// <summary>
    /// FrontPage
    /// </summary>
    public partial class FrontPage : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\FrontPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox titleText;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\FrontPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox yearText;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\FrontPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox type_combobox;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\FrontPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox genre_combobox;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\FrontPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox summaryText;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\FrontPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WrapPanel general_notes_area;
        
        #line default
        #line hidden
        
        
        #line 136 "..\..\FrontPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WrapPanel character_notes_area;
        
        #line default
        #line hidden
        
        
        #line 166 "..\..\FrontPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WrapPanel event_notes_area;
        
        #line default
        #line hidden
        
        
        #line 195 "..\..\FrontPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WrapPanel location_notes_area;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/citadel_wpf;component/frontpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\FrontPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.titleText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.yearText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.type_combobox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.genre_combobox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.summaryText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            
            #line 76 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Save_Media_Information);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 98 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.New_Note_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.general_notes_area = ((System.Windows.Controls.WrapPanel)(target));
            return;
            case 9:
            
            #line 127 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.New_Character_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 128 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Character_Relationship_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 129 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.NewImmediateFamilyTree);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 130 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.NewExtendedFamilyTree);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 131 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.NewFullFamilyTree);
            
            #line default
            #line hidden
            return;
            case 14:
            this.character_notes_area = ((System.Windows.Controls.WrapPanel)(target));
            return;
            case 15:
            
            #line 158 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.New_Event_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 159 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Event_Relationship_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 161 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.NewEventMap);
            
            #line default
            #line hidden
            return;
            case 18:
            this.event_notes_area = ((System.Windows.Controls.WrapPanel)(target));
            return;
            case 19:
            
            #line 188 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.New_Location_Click);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 189 "..\..\FrontPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.NewEventMap);
            
            #line default
            #line hidden
            return;
            case 21:
            this.location_notes_area = ((System.Windows.Controls.WrapPanel)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

