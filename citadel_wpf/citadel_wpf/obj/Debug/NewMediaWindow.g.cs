﻿#pragma checksum "..\..\NewMediaWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7B3AFA3FDFAD2BD8912D9187A445401A94DFC146"
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
    /// NewMediaWindow
    /// </summary>
    public partial class NewMediaWindow : citadel_wpf.NewEntityWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\NewMediaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock media_name;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\NewMediaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox name_text;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\NewMediaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock media_year;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\NewMediaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox year_text;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\NewMediaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock media_type;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\NewMediaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox type_combobox;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\NewMediaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock genre_text;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\NewMediaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox genre_combobox;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\NewMediaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox summary_text;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\NewMediaWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock required_text;
        
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
            System.Uri resourceLocater = new System.Uri("/citadel_wpf;component/newmediawindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\NewMediaWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.media_name = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.name_text = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.media_year = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.year_text = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.media_type = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.type_combobox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.genre_text = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.genre_combobox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            this.summary_text = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.required_text = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            
            #line 67 "..\..\NewMediaWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Cancel_and_Close);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 68 "..\..\NewMediaWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Save);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

